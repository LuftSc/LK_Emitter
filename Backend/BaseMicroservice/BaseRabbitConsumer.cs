using EmitterPersonalAccount.Core.Domain.SharedKernal;
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
        private readonly string rabbitUri;
        private IConnection connection;
        private IChannel channel;
        public BaseRabbitConsumer(string rabbitUri)
        {
            this.rabbitUri = rabbitUri;
        }
        public async Task ExecuteAsync(RabbitMqAction consumedAction, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory { Uri = new Uri(rabbitUri) };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Handler;

            await channel
                .BasicConsumeAsync(consumedAction.QueueName, autoAck: true, consumer, cancellationToken);
        }
        public abstract Task<Result> Handler(object model, BasicDeliverEventArgs args);
        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
