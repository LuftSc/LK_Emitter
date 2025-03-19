using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ExternalOrderReportsService.Consumers;
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

        private readonly RequestListOfShareholdersConsumer listLOSConsumer;

        public ConsumerRunService(IConfiguration configuration, IServiceProvider provider)
        {
            var rabbitUri = configuration.GetConnectionString("RabbitMqUri");

            ArgumentNullException.ThrowIfNullOrEmpty(rabbitUri,
                "Rabbit URI can not be null or empty!");


            listLOSConsumer = new RequestListOfShareholdersConsumer(rabbitUri, provider);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await listLOSConsumer
                .ExecuteAsync(RabbitMqAction.RequestListOfShareholders, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            listLOSConsumer.Dispose();

            return Task.CompletedTask;
        }
    }
}
