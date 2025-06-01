using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseMicroservice
{
    public abstract class RpcClientBase : IDisposable, IRpcClient
    {// Класс для микросервиса, инициирующего запрос
        private IConnection connection;
        private IChannel channel;
        // Имя временной очереди для ответов
        private string replyQueueName;
        // consumer для получения ответа от RpcServer
        private AsyncEventingBasicConsumer consumer;
        // словарь для сопоставления вопросов и ответов
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>>
            callbackMapper = new();
        private string rabbitUri { get; set; }
        public RpcClientBase(string rabbitURI)
        {
            rabbitUri = rabbitURI;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory() { Uri = new Uri(rabbitUri) };

            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            replyQueueName = channel.QueueDeclareAsync().Result.QueueName;

            // Создаём консьюмера
            consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += Handler;
            // Начинаем прослушивать временную очередь
            // (чтобы получить оттуда ответ от RpcServer)
            await channel.BasicConsumeAsync(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true
                );
        }
        public async Task<Result<TResult>> CallAsync<TResult>
            (string message, RabbitMqAction action, CancellationToken cancellationToken = default)
        {
            var props = new BasicProperties();
            var correlationId = Guid.NewGuid().ToString();

            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            var messagesBytes = Encoding.UTF8.GetBytes(message);
            // Для асинхронного ожидания ответа
            var tcs = new TaskCompletionSource<string>();
            // Сохраняем в словарь по ключу - CorrelationId

            callbackMapper.TryAdd(correlationId, tcs);
            // Тут отправляем наше сообщение в обычную очередь
            await channel.BasicPublishAsync(
                exchange: action.ExchangeName,
                routingKey: action.RoutingKey,
                basicProperties: props,
                body: messagesBytes,
                mandatory: false
                );
            // Отменяем ожидание, если сработал CancellationToken
            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));

            // Возвращаем Task,
            // который завершится когда прийдёт ответ
            var result = await tcs.Task;
            var typedResult = JsonSerializer.Deserialize<ResultDTO<TResult>>(result);

            if (typedResult is null || typedResult.Value is null)
                return Result<TResult>
                    .Error(new GettingResultFromRpcServerError<TResult>());

            return Result<TResult>.Success(typedResult.Value);
        }
        private async Task Handler(object model, BasicDeliverEventArgs args)
        {
            // Пробуем получить CorrelationId из свойств полученного сообщения
            if (!callbackMapper.TryRemove(args.BasicProperties.CorrelationId, out var tcs))
                return;

            var body = args.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            // Ставим результат для TaskCompletionSource
            tcs.TrySetResult(response);

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }

    public class GettingResultFromRpcServerError<TErrorType> : Error
    {
        public override string Type => nameof(TErrorType);
    }
    public record ResultDTO<TResult>(TResult Value) { }
}

