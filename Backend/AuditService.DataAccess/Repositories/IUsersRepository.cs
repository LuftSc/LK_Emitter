using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;

namespace AuditService.DataAccess.Repositories
{
    public interface IUsersRepository : IRepository<UserProjection>
    {
        Task<Result> AddUserWithEmittersBindings(UserProjection user, List<Guid> emittersId, CancellationToken cancellation);
        Task<Result> BindToEmitters(List<Guid> emittersListId, Guid userId, CancellationToken cancellation);


    }
}