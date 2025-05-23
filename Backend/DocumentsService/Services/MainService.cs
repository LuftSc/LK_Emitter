
using DocumentsService.Consumers;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace DocumentsService.Services
{
    public class MainService : IHostedService
    {
        private GetDocumentsInfoRpcServer getDocInfoRpcServer;
        private SendDocumentConsumer sendDocsConsumer;
        private DownloadDocumentRpcServer downloadDocRpcServer;
        private DeleteDocumentConsumer deleteDocConsumer;

        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            sendDocsConsumer = new SendDocumentConsumer(rabbitUri, provider);

            getDocInfoRpcServer = new GetDocumentsInfoRpcServer(rabbitUri, provider);

            downloadDocRpcServer = new DownloadDocumentRpcServer(rabbitUri, provider);

            deleteDocConsumer = new DeleteDocumentConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await sendDocsConsumer.ExecuteAsync(RabbitMqAction.SendDocument, cancellationToken);
            await getDocInfoRpcServer.StartAsync(RabbitMqAction.GetDocumentInfo, cancellationToken);
            await downloadDocRpcServer.StartAsync(RabbitMqAction.DownloadDocument, cancellationToken);
            await deleteDocConsumer.ExecuteAsync(RabbitMqAction.DeleteDocument, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            sendDocsConsumer.Dispose();
            getDocInfoRpcServer.Dispose();
            downloadDocRpcServer.Dispose();
            deleteDocConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
