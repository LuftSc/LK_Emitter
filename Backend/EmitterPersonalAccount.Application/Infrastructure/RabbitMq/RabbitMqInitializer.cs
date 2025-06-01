using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.RabbitMq
{
    public class RabbitMqInitializer
        (IOptions<RabbitMqInitOptions> options) : IHostedService
    {
        private IChannel channel;
        private IConnection connection;
        private readonly RabbitMqInitOptions options = options.Value;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(options.RabbitMqUri) };

            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            foreach (var exchange in options.Exchanges)
            {
                await channel.ExchangeDeclareAsync(
                    exchange: exchange.Name,
                    type: exchange.Type,
                    durable: true,
                    autoDelete: false,
                    arguments: null);
                foreach (var queue in exchange.Queues)
                {
                    await channel.QueueDeclareAsync(
                        queue: queue.Name,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    foreach (var key in queue.RoutingKeys)
                    {
                        await channel.QueueBindAsync(
                            queue: queue.Name,
                            exchange: exchange.Name,
                            routingKey: key,
                            arguments: null
                        );
                    }
                }
            }
            await channel.QueueDeclareAsync(
                        queue: "email",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

            await channel.QueueDeclareAsync(
                        queue: "outbox",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await channel.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}
