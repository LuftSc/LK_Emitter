using BaseMicroservice;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO.ListOSA;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
using ExternalOrderReportsService.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public class RequestOrderReportConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public RequestOrderReportConsumer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }

        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<RequestOrderReportEvent>
                .Deserialize(args);

            var result = Result.Success();
            using (var scope = provider.CreateScope())
            {
                var orderReportsService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                switch (ev.ReportType)
                {
                    case ReportType.ListOfShareholders:
                        result = await orderReportsService.RequestReport(
                            JsonSerializer.Deserialize<GenerateListOSARequest>(ev.RequestDataJSON),
                            ev.SendingDate,
                            ev.UserId);
                        break;
                    case ReportType.ReeRepNotSign:
                        result = await orderReportsService.RequestReport(
                            JsonSerializer.Deserialize<ReeRepNotSignRequest>(ev.RequestDataJSON),
                            ev.SendingDate,
                            ev.UserId );
                        break;
                    case ReportType.DividendList:
                        result = await orderReportsService.RequestReport(
                            JsonSerializer.Deserialize<ReportAboutDividendListNotSignRequest>(ev.RequestDataJSON),
                            ev.SendingDate,
                            ev.UserId);
                        break;
                    default:
                        return Result.Error(new UnsupportedReportGeneratingTypeError());
                }
            }
            return result;
        }
    }

    public class UnsupportedReportGeneratingTypeError : Error
    {
        public override string Type => nameof(UnsupportedReportGeneratingTypeError);
    }
}
