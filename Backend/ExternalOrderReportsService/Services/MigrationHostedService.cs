
using ExternalOrderReportService.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExternalOrderReportsService.Services
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
                .GetRequiredService<ExternalOrderReportServiceDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
