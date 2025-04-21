using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
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
            return Result.Success();
            /*var ev = EventDeserializer<RequestReeRepEvent>
                .Deserialize(args);

            var orderReportCreatingResult = OrderReport
                .Create("Список ЗЛ", ev.SendingDate, ev.RequestData.EmitId);

            var methodSendingResult = MethodResultSending.SendReeRepReport;

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
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value, methodSendingResult);
                    return setProcessingResult;
                }

                var updatedRequestData = ev.RequestData with
                {
                    GUID = orderReportCreatingResult.Value.Id.ToString()
                };

                var orderReportService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                var responseResult = await orderReportService
                    .RequestReeRepReport(ev.SendingDate, updatedRequestData);

                if (!responseResult.IsSuccessfull)
                {
                    await statusChangeService
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value, methodSendingResult);
                    return responseResult;
                }

                var statusSuccessResult = await statusChangeService
                    .SetSuccessfullStatus(
                        ev.UserId,
                        orderReportCreatingResult.Value,
                        responseResult.Value, methodSendingResult);

                if (!statusSuccessResult.IsSuccessfull)
                {
                    await statusChangeService
                        .SetFailedStatus(ev.UserId, orderReportCreatingResult.Value, methodSendingResult);
                    return statusSuccessResult;
                }

                return Result.Success();

            }*/
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
