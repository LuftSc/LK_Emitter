
using AuditService.Consumers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace AuditService.Services
{
    public class MainService : IHostedService
    {
        private AuditConsumer auditConsumer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            auditConsumer = new AuditConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await auditConsumer.ExecuteAsync(RabbitMqAction.Audit, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            auditConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
