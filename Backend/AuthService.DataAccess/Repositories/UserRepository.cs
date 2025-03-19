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

namespace AuthService.DataAccess.Repositories
{
    public sealed class UserRepository 
        : EFRepository<User, AuthDbContext>, IUserRepository
    {
        private readonly AuthDbContext context;

        public UserRepository(AuthDbContext context)
            : base(context) 
        {
            this.context = context;
        }

        public async Task<Result> UpdatePassword(Guid userId, string newHashedPassword)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(f => f
                    .SetProperty(p => p.PasswordHash, newHashedPassword));

            return Result.Success();
        }
    }
}
