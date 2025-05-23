using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PersonalData;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        Task<List<User>> GetUsersIncludeEmitters(
            Expression<Func<User, bool>> predicate,
            int page,
            int pageSize,
            CancellationToken cancellation);

        Task<Result> UnbindFromEmitter(
            Guid userId,
            Guid emitterId,
            CancellationToken cancellation
            );

        Task<Result<List<Emitter>>> GetEmittersCurrentUser(
            Guid userId,
            int page = 1,
            int pagesize = 10,
            CancellationToken cancellation = default);

        Task<Result<User>> GetUserWithRoles(Guid userId);
        Task<Result<User>> GetUserWithRoles(Expression<Func<User, bool>> predicate);

        Task<Result> Update(
            Guid userId,
            string encryptedEmail,
            string hashedEmail,
            string encryptFullName,
            string fullNameSearchHash,
            string encryptedBirthDate,
            EncryptedPassport encryptedPassport,
            string encryptedPhone);
    }
}
