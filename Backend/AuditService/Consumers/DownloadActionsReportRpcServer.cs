using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace AuditService.Consumers
{
    public class DownloadActionsReportRpcServer : RpcServerBase<Result<DocumentInfo>>
    {
        private readonly IServiceProvider provider;

        public DownloadActionsReportRpcServer
            (string rabbitUri, IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }

        public override async Task<Result<DocumentInfo>> OnMessageProcessingAsync
            (string message, BasicDeliverEventArgs args)
        {
            if (args.RoutingKey != "logs.download")
                throw new InvalidOperationException("Неверный RoutingKey");

            var reportId = JsonSerializer.Deserialize<Guid>(message);

            using (var scope = provider.CreateScope())
            {
                var auditRepository = scope.ServiceProvider
                    .GetRequiredService<IAuditRepository>();

                var reportGettingResult = await auditRepository
                    .GetActionsReportById(reportId);

                if (!reportGettingResult.IsSuccessfull)
                    return Result<DocumentInfo>
                        .Error(new ActionsReportDownloadError());

                var result = new DocumentInfo()
                {
                    Content = reportGettingResult.Value.Content,
                    FileName = reportGettingResult.Value.Title,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                return Result<DocumentInfo>.Success(result);
            }
            
        }
    }

    public class ActionsReportDownloadError : Error
    {
        public override string Type => nameof(ActionsReportDownloadError);
    }
}
