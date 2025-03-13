using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IRpcClient
    {
        Task<Result<TResult>> CallAsync<TResult>
            (string message, RabbitMqAction action, CancellationToken cancellationToken = default);
        void Dispose();
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}