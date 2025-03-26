using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseMicroservice
{
    public abstract class BaseRabbitPublisher : IRabbitMqPublisher
    {
        private readonly ConnectionFactory factory;
        public BaseRabbitPublisher(IConfiguration configuration)
        {
            var uri = configuration.GetConnectionString("RabbitMqUri")
                ?? throw new ArgumentNullException("RabbitMqUri conn str is null");

            factory = new ConnectionFactory
            {
                Uri = new Uri(uri)
            };
        }
        public async Task<bool> SendMessageAsync(string message, RabbitMqAction action,
            CancellationToken cancellation)
        {
            try
            {
                await using var connection = await factory.CreateConnectionAsync(cancellation);
                await using var channel = await connection.CreateChannelAsync(options: null, cancellation);

                var body = Encoding.UTF8.GetBytes(message);

                await channel
                    .BasicPublishAsync(action.ExchangeName, action.RoutingKey, false, body, cancellation);
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
        public async Task<bool> SendMessageAsync(string message, string queueName,
            CancellationToken cancellation)
        {
            try
            {
                await using var connection = await factory.CreateConnectionAsync(cancellation);
                await using var channel = await connection.CreateChannelAsync(options: null, cancellation);
                /*await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellation);*/

                var body = Encoding.UTF8.GetBytes(message);

                await channel
                    .BasicPublishAsync("", queueName, false, body, cancellation);

                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}

