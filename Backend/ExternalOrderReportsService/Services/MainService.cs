
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ExternalOrderReportsService.Consumers;

namespace ExternalOrderReportsService.Services
{
    public class MainService : IHostedService
    {
        private readonly RequestListOfShareholdersConsumer requestShareholdersConsumer;
        private readonly RequestReeRepConsumer requestReeRepConsumer;
        private readonly RequestDividendListConsumer requestDividendListConsumer;

        private readonly DownloadReportOrderRpcServer downloadReportOrderRpcServer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            requestShareholdersConsumer = new RequestListOfShareholdersConsumer(rabbitUri, provider);
            requestReeRepConsumer = new RequestReeRepConsumer(rabbitUri, provider);
            requestDividendListConsumer = new RequestDividendListConsumer(rabbitUri, provider);

            downloadReportOrderRpcServer = new DownloadReportOrderRpcServer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await requestShareholdersConsumer
                .ExecuteAsync(RabbitMqAction.RequestListOfShareholders, cancellationToken);
            await requestReeRepConsumer
                .ExecuteAsync(RabbitMqAction.RequestReeRep, cancellationToken);
            await requestDividendListConsumer
                .ExecuteAsync(RabbitMqAction.RequestDividendList, cancellationToken);

            await downloadReportOrderRpcServer
                .StartAsync(RabbitMqAction.DownloadReportOrder, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            requestShareholdersConsumer.Dispose();
            requestReeRepConsumer.Dispose();
            requestDividendListConsumer.Dispose();

            downloadReportOrderRpcServer.Dispose();

            return Task.CompletedTask;
        }
    }
}
