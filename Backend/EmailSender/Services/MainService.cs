
using EmailSender.Consumers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace EmailSender.Services
{
    public class MainService : IHostedService
    {
        AuthConfirmationConsumer consumer;
        public MainService(string rabbitUri, ISender sender)
        {
            consumer = new AuthConfirmationConsumer(rabbitUri, sender);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await consumer.ExecuteAsync(RabbitMqAction.SendEmailConfirmation, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
