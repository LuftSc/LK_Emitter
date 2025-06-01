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
        private bool _autoAck = true;
        public BaseRabbitConsumer(string rabbitUri)
        {
            this.rabbitUri = rabbitUri;
        }
        public async Task ExecuteAsync
            (RabbitMqAction consumedAction, 
            CancellationToken cancellationToken, 
            bool autoAck = true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory { Uri = new Uri(rabbitUri) };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);

            _autoAck = autoAck;

            consumer.ReceivedAsync += BaseHandler;

            await channel
                .BasicConsumeAsync(consumedAction.QueueName, autoAck: autoAck, consumer, cancellationToken);
        }
        private async Task BaseHandler(object model, BasicDeliverEventArgs args)
        {
            if (_autoAck) await Handler(model, args);
            else
            {
                try
                {
                    await Handler(model, args);
                    await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
                }
                catch (InvalidOperationException) // Для Nack с Requeue
                {
                    await channel.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
                catch // Для Nack без Requeue
                {
                    await channel.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: false);
                }
            }
        }
        public abstract Task<Result> Handler(object model, BasicDeliverEventArgs args);
        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
