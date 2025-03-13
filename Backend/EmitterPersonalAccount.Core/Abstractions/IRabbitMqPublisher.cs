//using Microsoft.Extensions.Configuration;

using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IRabbitMqPublisher
    {
        Task<bool> SendMessageAsync(string message, RabbitMqAction action,
            CancellationToken cancellation);
        Task<bool> SendMessageAsync(string message, string queueName,
            CancellationToken cancellation);
    }
}