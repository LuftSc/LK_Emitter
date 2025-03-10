
using EmailSender.Consumers;

namespace EmailSender.Services
{
    public class MainService : IHostedService
    {
        AuthConfirmationConsumer consumer;
        public MainService(string rabbitUri, string queueName, ISender sender)
        {
            consumer = new AuthConfirmationConsumer(rabbitUri, queueName, sender);
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
