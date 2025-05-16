using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        
        public async Task<Result> AddWithRole
            (User user, Role role, List<Guid>? emittersIdList, CancellationToken cancellation)
        {
            var roleEntity = await context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)role);

            if (roleEntity is null) 
                return Result.Error(new UnsupportedUserRoleError());

            user.Roles = [roleEntity];

            if (role == Role.Emitter && emittersIdList is not null) 
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
        
    }

    public class UnsupportedUserRoleError : Error
    {
        public override string Type => nameof(UnsupportedUserRoleError);
    }
}
