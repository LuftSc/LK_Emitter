
using AuditService.Consumers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace AuditService.Services
{
    public class MainService : IHostedService
    {
        private AuditConsumer auditConsumer;
        private UpdateDataConsumer updateDataConsumer;
        public MainService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            auditConsumer = new AuditConsumer(rabbitUri, provider);
            updateDataConsumer = new UpdateDataConsumer(rabbitUri, provider);   
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await auditConsumer.ExecuteAsync(RabbitMqAction.Audit, cancellationToken);
            await updateDataConsumer.ExecuteAsync(RabbitMqAction.SendOutboxMessage, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            auditConsumer.Dispose();
            updateDataConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
