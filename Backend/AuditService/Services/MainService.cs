
using AuditService.Consumers;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace AuditService.Services
{
    public class MainService : IHostedService
    {
        private AuditConsumer auditConsumer;
        private UpdateDataConsumer updateDataConsumer;

        private IRpcClient auditRpcClient;
        private GetLogsConsumer getLogsConsumer;

        private GetActionsReportsRpcServer getActionsReportsRpcServer;
        private DownloadActionsReportRpcServer downloadActionsReportRpcServer;
        //private GetLogsRpcServer getLogsRpcServer;
        public MainService(IConfiguration configuration, IServiceProvider provider, IRpcClient auditRpcClient)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            auditConsumer = new AuditConsumer(rabbitUri, provider);
            updateDataConsumer = new UpdateDataConsumer(rabbitUri, provider);
            getLogsConsumer = new GetLogsConsumer(rabbitUri, provider);

            this.auditRpcClient = auditRpcClient;

            getActionsReportsRpcServer = new GetActionsReportsRpcServer(rabbitUri, provider);
            downloadActionsReportRpcServer = new DownloadActionsReportRpcServer(rabbitUri, provider);
            //getLogsRpcServer = new GetLogsRpcServer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await auditConsumer.ExecuteAsync(RabbitMqAction.WriteUsersLogs, cancellationToken);
            await updateDataConsumer.ExecuteAsync(RabbitMqAction.SendOutboxMessage, cancellationToken);
            await getLogsConsumer.ExecuteAsync(RabbitMqAction.CollectUserLogs, cancellationToken, autoAck: false);

            await auditRpcClient.InitializeAsync(cancellationToken);

            await getActionsReportsRpcServer
                .StartAsync(
                    RabbitMqAction.GetActionsReports, 
                    cancellationToken);

            await downloadActionsReportRpcServer
                .StartAsync(
                    RabbitMqAction.DownloadActionsReport, 
                    cancellationToken);
            //await getLogsRpcServer.StartAsync(RabbitMqAction.CollectUserLogs, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            auditConsumer.Dispose();
            updateDataConsumer.Dispose();
            getLogsConsumer.Dispose();

            auditRpcClient.Dispose();

            getActionsReportsRpcServer.Dispose();
            downloadActionsReportRpcServer.Dispose();
           // getLogsRpcServer.Dispose();

            return Task.CompletedTask;
        }
    }
}
