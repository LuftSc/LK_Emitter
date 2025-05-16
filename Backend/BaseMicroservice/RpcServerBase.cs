using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace BaseMicroservice
{
    public abstract class RpcServerBase<TResult> : IDisposable, IRpcServer<TResult>
        where TResult : Result
    {
        protected RpcServerBase(string rabbitUri)
        {
            this.rabbitUri = rabbitUri;
        }
        private IChannel channel;
        private IConnection connection;
        private readonly string rabbitUri;
        //private readonly string queueName;

        public abstract Task<TResult> OnMessageProcessingAsync(string message);
        public abstract Task<TResult> OnMessageProcessingFailureAsync(Exception exception);
        public async Task StartAsync(RabbitMqAction consumedAction, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory() { Uri = new Uri(rabbitUri)};

            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            // Настраиваем так, чтобы за раз обрабатывалось только 1 сообщение
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            // Создаём консьюмера для обработки запросов
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += Handler;

            // Начинаем слушать очередь для получения запросов
            await channel.BasicConsumeAsync(queue: consumedAction.QueueName, 
                autoAck: false, consumer: consumer, cancellationToken);
        }
        private async Task Handler(object model, BasicDeliverEventArgs args)
        {
            //string response = null;
            TResult response = null;

            // Достаём тело сообщения и пропсы
            var body = args.Body.ToArray();
            var props = args.BasicProperties;

            var replyProps = new BasicProperties();
            // Сохраняем для ответа такой же CorrelationId,
            // какой и был у запроса
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(body);
                response = await OnMessageProcessingAsync(message);
            }
            catch (Exception exception)
            {
                response = await OnMessageProcessingFailureAsync(exception);
            }
            finally
            {
                var responseString = JsonSerializer.Serialize(response);
                var responseBytes = Encoding.UTF8.GetBytes(responseString);

                // Отправляем ответ во временную очередь (ReplyTo)
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: props.ReplyTo,
                    basicProperties: replyProps,
                    body: responseBytes,
                    mandatory: false
                    );
                // Подтверждаем обработку сообщения
                await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
            }
        }
        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
