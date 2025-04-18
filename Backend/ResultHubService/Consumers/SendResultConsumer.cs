using BaseMicroservice;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;
using ResultHubService.Services;
using System.Text.Json;

namespace ResultHubService.Consumers
{
    public class SendResultConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public SendResultConsumer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<SendResultToClientEvent>
                .Deserialize(args);

            using(var scope = provider.CreateScope())
            {
                var resultService = scope.ServiceProvider.GetRequiredService<ResultService>();

                switch (ev.MethodForResultSending)
                {
                    case MethodResultSending.GetReports:
                        await resultService.SendReportsToClient(JsonSerializer
                            .Deserialize<ReportsPaginationListContent>(ev.ContentJSON));
                        break;

                    case MethodResultSending.SendListOSAReport:
                        await resultService.SendListOfShareholdersResultToClient(
                            JsonSerializer.Deserialize<OrderReportDTO>(ev.ContentJSON));

                    break;

                    case MethodResultSending.SendReeRepReport:
                        await resultService
                            .SendReeRepResultToClient(
                                JsonSerializer.Deserialize<OrderReportDTO>(ev.ContentJSON));
                    break;

                    case MethodResultSending.SendDividendListReport:
                        await resultService
                            .SendDividendListResultToClient(
                                JsonSerializer.Deserialize<OrderReportDTO>(ev.ContentJSON));
                    break;
                    default:
                        return await Task.FromResult
                            (Result.Error(new IncorrectMethodNameForSendingResult()));
                }

                return Result.Success();
            }
        }
    }
    public class IncorrectMethodNameForSendingResult : Error
    {
        public override string Type => nameof(IncorrectMethodNameForSendingResult);
    }
}
