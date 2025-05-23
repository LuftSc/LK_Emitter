
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ExternalOrderReportsService.Consumers;

namespace ExternalOrderReportsService.Services
{
    public class MainService : IHostedService
    {
        //private readonly GetOrderReportsConsumer getOrderReportsConsumer;

        private readonly RequestOrderReportConsumer requestOrderReportConsumer;

        private readonly DownloadReportOrderRpcServer downloadReportOrderRpcServer;

        private readonly GetOrderReportsRpcServer getOrderReportsRpcServer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            //getOrderReportsConsumer = new GetOrderReportsConsumer(rabbitUri, provider);
            getOrderReportsRpcServer = new GetOrderReportsRpcServer(rabbitUri, provider);

            requestOrderReportConsumer = new RequestOrderReportConsumer(rabbitUri, provider);

            downloadReportOrderRpcServer = new DownloadReportOrderRpcServer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            /*await getOrderReportsConsumer
                .ExecuteAsync(RabbitMqAction.GetOrderReports, cancellationToken);*/
            await getOrderReportsRpcServer
                .StartAsync(RabbitMqAction.GetOrderReports, cancellationToken);

            await requestOrderReportConsumer
                .ExecuteAsync(RabbitMqAction.RequestOrderReport, cancellationToken);

            await downloadReportOrderRpcServer
                .StartAsync(RabbitMqAction.DownloadReportOrder, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            /* requestShareholdersConsumer.Dispose();
             requestReeRepConsumer.Dispose();
             requestDividendListConsumer.Dispose();*/
            //getOrderReportsConsumer.Dispose();
            getOrderReportsRpcServer.Dispose();
            requestOrderReportConsumer.Dispose();
            downloadReportOrderRpcServer.Dispose();

            return Task.CompletedTask;
        }
    }
}
