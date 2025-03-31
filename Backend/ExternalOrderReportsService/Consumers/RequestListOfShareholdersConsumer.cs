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
    public record RequestListOfShareholdersEvent(
        DateTime SendingDate,
        ListOfShareholdersRequest RequestData,
        string UserId,
        Guid DocumentId
        ) { }

    public record ResultListOfShareholsders(
        DateTime SendingDate,
        Guid ExternalDocumentId,
        string UserId,
        Guid DocumentId
        )
    { }
    public class RequestListOfShareholdersConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public RequestListOfShareholdersConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler
            (object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<RequestListOfShareholdersEvent>
                .Deserialize(args);

            using (var scope = provider.CreateScope()) 
            {
                try
                {
                    var orderReportService = scope.ServiceProvider
                        .GetRequiredService<IOrderReportsService>();

                    var orderReportResult = await orderReportService
                        .RequestListOfShareholdersForMeetingReport(ev.SendingDate, ev.RequestData);

                    if (!orderReportResult.IsSuccessfull)
                        return Result.Error(new ListOfShareholsdersReportGeneratingError());

                    var response = new ResultListOfShareholsders
                        (ev.SendingDate, 
                        orderReportResult.Value, 
                        ev.UserId,
                        ev.DocumentId);

                    var message = JsonSerializer.Serialize(response);

                    var publisher = scope.ServiceProvider
                        .GetRequiredService<IRabbitMqPublisher>();

                    var isSuccessfull = await publisher
                        .SendMessageAsync(message, RabbitMqAction.ResultListOfShareholders, default);

                    if (!isSuccessfull)
                        return Result.Error(new ListOfShareholsdersReportQueueDeliveryError());

                    return Result.Success();
                }
                catch (InvalidOperationException ex)
                {
                    return Result.Error(new ListOfShareholsdersReportQueueDeliveryError());
                }
            }
        }
    }
    public class ListOfShareholsdersReportGeneratingError : Error
    {
        public override string Type => nameof(ListOfShareholsdersReportGeneratingError);
    }

    public class ListOfShareholsdersReportQueueDeliveryError : Error
    {
        public override string Type => nameof(ListOfShareholsdersReportQueueDeliveryError);
    }
}
