using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
//using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportService.DataAccess.Repositories;
using ExternalOrderReportsService.Contracts;

//using ExternalOrderReportsService.Contracts;
using ExternalOrderReportsService.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public record RequestListOfShareholdersEvent(
        DateTime SendingDate,
        ListOfShareholdersRequest RequestData,
        string UserId
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

            var orderReportCreatingResult = OrderReport
                .Create("Лист участников собрания акционеров", ev.SendingDate, ev.RequestData.IssuerId);

            var methodSendingResult = MethodResultSending.SendListOSAReport;

            if (!orderReportCreatingResult.IsSuccessfull) return orderReportCreatingResult;

            using (var scope = provider.CreateScope()) 
            {
                var statusChangeService = scope.ServiceProvider
                    .GetRequiredService<IReportStatusChangeService>();

                var setProcessingResult = await statusChangeService
                    .SetProcessingStatus(ev.UserId, orderReportCreatingResult.Value, methodSendingResult);

                if (!setProcessingResult.IsSuccessfull)
                {
                    await statusChangeService
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value , methodSendingResult);
                    return setProcessingResult;
                }

                var updatedRequestData = ev.RequestData with
                {
                    Guid = orderReportCreatingResult.Value.Id.ToString()
                };

                var orderReportService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                var responseResult = await orderReportService
                    .RequestListOfShareholdersForMeetingReport(ev.SendingDate, updatedRequestData);

                if (!responseResult.IsSuccessfull)
                {
                    await statusChangeService
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value , methodSendingResult);
                    return responseResult;
                }

                var statusSuccessResult = await statusChangeService
                    .SetSuccessfullStatus(
                        ev.UserId, 
                        orderReportCreatingResult.Value, 
                        responseResult.Value , methodSendingResult);

                if (!statusSuccessResult.IsSuccessfull)
                {
                    await statusChangeService
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value , methodSendingResult);
                    return statusSuccessResult;
                }

                return Result.Success();
               
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
