using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Application.Services
{
    public interface IOutboxService
    {
        Task<Result> CreateAndSaveOutboxMessage(OutboxMessageType type, string contentJSON, CancellationToken cancellationToken);
    }
}