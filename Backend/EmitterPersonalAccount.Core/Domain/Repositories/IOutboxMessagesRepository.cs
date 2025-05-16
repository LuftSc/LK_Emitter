using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Domain.Repositories
{
    public interface IOutboxMessagesRepository
    {
        Task<Result<List<OutboxMessage>>> GetNewlyAdded(CancellationToken cancellation);
        Task<Result> SetStatusFailed(Guid id, string error, CancellationToken cancellation);
        Task<Result> SetStatusProcessed(Guid id, CancellationToken cancellation);

        Task<Result> AddAsync(OutboxMessage message, CancellationToken cancellationToken);
    }
}