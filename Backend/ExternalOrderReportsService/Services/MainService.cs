
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ExternalOrderReportsService.Consumers;

namespace ExternalOrderReportsService.Services
{
    public class MainService : IHostedService
    {
        private readonly RequestListOfShareholdersConsumer requestShareholdersConsumer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            requestShareholdersConsumer = new RequestListOfShareholdersConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await requestShareholdersConsumer
                .ExecuteAsync(RabbitMqAction.RequestListOfShareholders, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            requestShareholdersConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
