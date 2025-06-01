using EmitterPersonalAccount.Application.Infrastructure.Consumers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider provider;
        private DecryptPersonalDataRpcServer decryptRpcServer;

        public MigrationHostedService(IConfiguration configuration, IServiceProvider provider)
        {
            this.provider = provider;
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");
            decryptRpcServer = new DecryptPersonalDataRpcServer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<EmitterPersonalAccountDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);

            await decryptRpcServer
                .StartAsync(RabbitMqAction.GetUserPersonalInfoForLogs, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            decryptRpcServer.Dispose();

            return Task.CompletedTask;
        }
    }
}
