using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PersonalData;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Repositories
{
    public sealed class UsersRepository
        : EFRepository<User, EmitterPersonalAccountDbContext>, IUserRepository
    {
        private readonly EmitterPersonalAccountDbContext context;

        public UsersRepository(EmitterPersonalAccountDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<Result<List<User>>> GetByListGuids(
            List<Guid> usersGuidList, 
            CancellationToken cancellation)
        {
            var users = await context.Users
                .AsNoTracking()
                .Where(u => usersGuidList.Contains(u.Id))
                .ToListAsync(cancellation);

            return Result<List<User>>.Success(users);
        }
        public async Task<Result> AddWithRole
            (User user, Role role, List<Guid>? emittersIdList, CancellationToken cancellation)
        {
            var roleEntity = await context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)role, cancellation);

            if (roleEntity is null) 
                return Result.Error(new UnsupportedUserRoleError());

            user.Roles = [roleEntity];

            if (role == Role.Emitter && emittersIdList is not null && emittersIdList.Count > 0) 
            {
                foreach (var emitterId in emittersIdList)
                {
                    var emitter = await context.Emitters.FindAsync(emitterId, cancellation);

                    if (emitter is null) return Result.Error(new EmitterNotFoundError());

                    user.Emitters.Add(emitter);
                }
            }

            await AddAsync(user, cancellation);

            await context.SaveChangesAsync(cancellation);

            return Result.Success();
        }
        public async Task<Result> AddRoleToUser
            (Guid userId, Role role, List<Guid>? emittersIdList, CancellationToken cancellation)
        {
            var user = await context.Users
                .FindAsync(userId, cancellation);

            if (user is null) return Result.Error(new UserNotFoundError());

            var roleEntity = await context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)role);

            if (roleEntity is null)
                return Result.Error(new UnsupportedUserRoleError());

            if (!user.Roles.Contains(roleEntity))
                user.Roles.Add(roleEntity);

            if (role == Role.Emitter && emittersIdList is not null)
            {
                var bindingResult = await BindToEmitters(userId, emittersIdList, cancellation);
                if (!bindingResult.IsSuccessfull) return bindingResult;
            }
            
            return Result.Success();
        }

        public async Task<Result> UnbindFromEmitter(
            Guid userId,
            Guid emitterId,
            CancellationToken cancellation
            )
        {
            var binding = await context.UserEmitter
                .Where(b => b.UserId == userId && b.EmitterId == emitterId)
                .ExecuteDeleteAsync(cancellation);

            return Result.Success();
        }
        public async Task<Result> BindToEmitters(
            Guid userId, 
            List<Guid> emittersIdList, 
            CancellationToken cancellation)
        {
            var user = await context.Users
                .AsNoTracking()
                .Include(u => u.Emitters)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null) return Result.Error(new UserNotFoundError());
            
            foreach (var emitterId in emittersIdList)
            {
                if (user.Emitters.Any(e => e.Id == emitterId)) continue;

                var binding = UserEmitter.Create(userId, emitterId);

                if (binding.IsSuccessfull) await context.AddAsync(binding.Value);
            }

            await context.SaveChangesAsync(cancellation);

            return Result.Success();
        }

        public async Task<Result> UpdatePassword(Guid userId, string newHashedPassword)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(f => f
                    .SetProperty(p => p.PasswordHash, newHashedPassword));

            return Result.Success();
        }

        public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
        {
            var roles = await context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions) 
                .Where(u => u.Id == userId)
                .Select(u => u.Roles) 
                .ToListAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions) 
                .Select(p => (Permission)p.Id) 
                .ToHashSet();
        }
        public async Task<Result<List<Emitter>>> GetEmittersCurrentUser(
            Guid userId, 
            int page = 1, 
            int pageSize = 10, 
            CancellationToken cancellation = default)
        {
            var userWithEmitters = await context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .Include(u => u.Emitters)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellation);

            if (userWithEmitters is null) 
                return Result<List<Emitter>>
                    .Error(new UserNotFoundError());

            if ((Role)userWithEmitters.Roles.Max().Id == Role.Registrator)
            {
                var allEmitters = await context.Emitters
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<List<Emitter>>.Success(allEmitters);
            } 
            else
            {
                var emitters = userWithEmitters.Emitters
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Result<List<Emitter>>.Success(emitters);
            }
        }
        public async Task<Result<User>> GetUserWithRoles(Guid userId)
        {
            var user = await context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) 
                return Result<User>.Error(new UserNotFoundError());

            return Result<User>.Success(user);
        }

        public async Task<Result<User>> GetUserWithRoles(Expression<Func<User, bool>> predicate)
        {
            var user = await context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(predicate);

            if (user is null)
                return Result<User>.Error(new UserNotFoundError());

            return Result<User>.Success(user);
        }
        public async Task<Result> Update(
            Guid userId,
            string encryptedEmail,
            string hashedEmail,
            string encryptFullName,
            string fullNameSearchHash,
            string encryptedBirthDate,
            EncryptedPassport encryptedPassport,
            string encryptedPhone
            )
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.EncryptedEmail, encryptedEmail)
                    .SetProperty(p => p.EmailSearchHash, hashedEmail)
                    .SetProperty(p => p.EncryptedFullName, encryptFullName)
                    .SetProperty(p => p.FullNameSearchHash, fullNameSearchHash)
                    .SetProperty(p => p.EncryptedBirthDate, encryptedBirthDate)
                    .SetProperty(p => p.EncryptedPassport.Series, encryptedPassport.Series)
                    .SetProperty(p => p.EncryptedPassport.Number, encryptedPassport.Number)
                    .SetProperty(p => p.EncryptedPassport.DateOfIssue, encryptedPassport.DateOfIssue)
                    .SetProperty(p => p.EncryptedPassport.Issuer, encryptedPassport.Issuer)
                    .SetProperty(p => p.EncryptedPassport.UnitCode, encryptedPassport.UnitCode)
                    .SetProperty(p => p.EncryptedPhone, encryptedPhone));

            return Result.Success();
        }

        public async Task<List<User>> GetUsersIncludeEmitters(
            Expression<Func<User, bool>> predicate,
            int page,
            int pageSize,
            CancellationToken cancellation)
        {
            var query = context.Users
                .AsNoTracking()
                .Include(u => u.Emitters)
                .Include(u => u.Roles)
                .Where(predicate);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellation);
        }
        
    }

    public class UnsupportedUserRoleError : Error
    {
        public override string Type => nameof(UnsupportedUserRoleError);
    }
}
