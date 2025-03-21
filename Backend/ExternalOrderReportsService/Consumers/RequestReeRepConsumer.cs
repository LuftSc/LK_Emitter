using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
using ExternalOrderReportsService.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public record RequestReeRepEvent(
        DateTime SendingDate,
        ReeRepNotSignRequest RequestData,
        string UserId
        )
    { }
    public record ResultReeRep(
        DateTime SendingDate,
        Guid DocumentId,
        string UserId
        )
    { }
    public class RequestReeRepConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public RequestReeRepConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }

        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<RequestReeRepEvent>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var orderReportService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                var orderReportResult = await orderReportService
                    .RequestReeRepReport(ev.SendingDate, ev.RequestData);

                if (!orderReportResult.IsSuccessfull)
                    return Result.Error(new ReeRepReportGeneratingError());

                var response = new ResultReeRep
                    (ev.SendingDate, orderReportResult.Value, ev.UserId);

                var message = JsonSerializer.Serialize(response);

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IRabbitMqPublisher>();

                var isSuccessfull = await publisher
                    .SendMessageAsync(message, RabbitMqAction.ResultReeRep, default);

                if (!isSuccessfull)
                    return Result.Error(new ReeRepReportQueueDeliveryError());

                return Result.Success();
            }
        }
    }

    public class ReeRepReportGeneratingError : Error
    {
        public override string Type => nameof(ReeRepReportGeneratingError);
    }
    public class ReeRepReportQueueDeliveryError : Error
    {
        public override string Type => nameof(ReeRepReportQueueDeliveryError);
    }
}
