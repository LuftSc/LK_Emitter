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
