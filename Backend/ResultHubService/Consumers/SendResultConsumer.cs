using BaseMicroservice;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
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
                    case "ListOfShareholders":
                        await resultService.SendListOfShareholdersResultToClient(
                            JsonSerializer.Deserialize<SendListOSAResultContent>(ev.ContentJSON));

                    break;

                    case "GetReports":
                        await resultService.SendReportsToClient(JsonSerializer
                            .Deserialize<ReportsPaginationListContent>(ev.ContentJSON));
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
