using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IRpcServer<TResult>
        where TResult : Result
    {
        void Dispose();
        Task<TResult> OnMessageProcessingAsync(string message);
        Task<TResult> OnMessageProcessingFailureAsync(Exception exception);
        Task StartAsync(RabbitMqAction consumedAction, CancellationToken cancellationToken);
    }
}