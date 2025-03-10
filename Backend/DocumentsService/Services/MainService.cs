
using DocumentsService.Consumers;
using EmitterPersonalAccount.Core.Abstractions;

namespace DocumentsService.Services
{
    public class MainService : IHostedService
    {
        private SendDocumentConsumer consumer;
        public MainService(string rabbitUri, string queueName, IServiceProvider provider)
        {
            consumer = new SendDocumentConsumer(rabbitUri, queueName, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await consumer.ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
