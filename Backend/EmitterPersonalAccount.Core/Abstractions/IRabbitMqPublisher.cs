//using Microsoft.Extensions.Configuration;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IRabbitMqPublisher
    {
        Task<bool> SendMessageAsync(string message, string queue, CancellationToken cancellation = default);
    }
}