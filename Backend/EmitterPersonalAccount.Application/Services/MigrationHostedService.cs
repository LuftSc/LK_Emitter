using EmitterPersonalAccount.DataAccess;
using Microsoft.EntityFrameworkCore;
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

        public MigrationHostedService(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<EmitterPersonalAccountDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
