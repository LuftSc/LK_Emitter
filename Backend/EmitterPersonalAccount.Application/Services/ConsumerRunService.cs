using EmitterPersonalAccount.Application.Infrastructure.Consumers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
//using ExternalOrderReportsService.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class ConsumerRunService : IHostedService
    {
        private readonly ResultListOfShareholdersConsumer listLOSConsumer;
        private readonly ResultReeRepConsumer reeRepConsumer;
        private readonly ResultDividendListConsumer dividendListConsumer;
        public ConsumerRunService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");

            ArgumentNullException.ThrowIfNullOrEmpty(rabbitUri,
                "Rabbit URI can not be null or empty!");
            
            listLOSConsumer = new ResultListOfShareholdersConsumer(rabbitUri, provider);
            reeRepConsumer = new ResultReeRepConsumer(rabbitUri, provider);
            dividendListConsumer = new ResultDividendListConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await listLOSConsumer
                .ExecuteAsync(RabbitMqAction.ResultListOfShareholders, cancellationToken);
            await reeRepConsumer
                .ExecuteAsync(RabbitMqAction.ResultReeRep, cancellationToken);
            await dividendListConsumer
                .ExecuteAsync(RabbitMqAction.ResultDividendList, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            listLOSConsumer.Dispose();
            reeRepConsumer.Dispose();
            dividendListConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
