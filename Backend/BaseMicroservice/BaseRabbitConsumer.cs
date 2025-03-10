using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseMicroservice
{
    public abstract class BaseRabbitConsumer : IDisposable
    {
        private IConnection connection;
        private IChannel channel;
        public BaseRabbitConsumer(string rabbitUri, string queueName)
        {
            RabbitUri = rabbitUri;
            QueueName = queueName;
        }

        public string RabbitUri { get; }
        public string QueueName { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory { Uri = new Uri(RabbitUri) };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                    queue: QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Handler;

            await channel.BasicConsumeAsync(QueueName, autoAck: true, consumer, cancellationToken);
        }
        public abstract Task<Result> Handler(object model, BasicDeliverEventArgs args);
        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
