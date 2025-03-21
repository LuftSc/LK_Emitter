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
    public record RequestDividendListEvent(
        DateTime SendingDate,
        ReportAboutDividendListNotSignRequest RequestData,
        string UserId
        )
    { }
    public record ResultDividendList(
        DateTime SendingDate,
        Guid DocumentId,
        string UserId
        )
    { }
    public class RequestDividendListConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public RequestDividendListConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<RequestDividendListEvent>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var orderReportService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                var orderReportResult = await orderReportService
                    .RequestDividendListReport(ev.SendingDate, ev.RequestData);

                if (!orderReportResult.IsSuccessfull)
                    return Result.Error(new DividendListReportGeneratingError());

                var response = new ResultDividendList
                    (ev.SendingDate, orderReportResult.Value, ev.UserId);

                var message = JsonSerializer.Serialize(response);

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IRabbitMqPublisher>();

                var isSuccessfull = await publisher
                    .SendMessageAsync(message, RabbitMqAction.ResultDividendList, default);

                if (!isSuccessfull)
                    return Result.Error(new DividendListReportQueueDeliveryError());

                return Result.Success();
            }
        }
    }

    public class DividendListReportGeneratingError : Error
    {
        public override string Type => nameof(DividendListReportGeneratingError);
    }
    public class DividendListReportQueueDeliveryError : Error
    {
        public override string Type => nameof(DividendListReportQueueDeliveryError);
    }
}
