using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<Result> UpdatePassword(Guid userId, string newHashedPassword);
        Task<HashSet<Permission>> GetUserPermissions(Guid userId);
        Task<Result> AddWithRole
            (User user, Role role, List<Guid>? emittersIdList, CancellationToken cancellation);
        Task<Result> AddRoleToUser
            (Guid userId, Role role, List<Guid>? emittersIdList, CancellationToken cancellation);
        Task<Result> BindToEmitters(
            Guid userId,
            List<Guid> emittersIdList,
            CancellationToken cancellation);
    }
}
