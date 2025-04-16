using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ResultHubService.Consumers;

namespace ResultHubService.Services
{
    public class MainService : IHostedService
    {
        private SendResultConsumer resultConsumer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            resultConsumer = new SendResultConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await resultConsumer
                .ExecuteAsync(RabbitMqAction.SendResultToClient, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            resultConsumer.Dispose();
            await Task.CompletedTask;
        }
    }
}
