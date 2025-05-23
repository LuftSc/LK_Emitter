using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
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
    public class RegistratorRepository 
        : EFRepository<Registrator, EmitterPersonalAccountDbContext>, 
        IRegistratorRepository
    {
        private readonly EmitterPersonalAccountDbContext context;

        public RegistratorRepository(EmitterPersonalAccountDbContext context) : base(context)
        {
            this.context = context;
        }

       /* public async Task<Result> BindUser(Guid registratorId, Guid userId)
        {
            var user = await context.Users
                .Include(u => u.Registrator)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return Result.Error(new UserNotFoundError());

            var registrator = await FindAsync([registratorId], default);

            if (registrator is null)
                return Result.Error(new RegistratorNotFoundError());

            if (user.Registrator is not null)
                return Result.Error(new UserAlreadyBoundedError());

            user.Registrator = registrator;

            await context.SaveChangesAsync();

            return Result.Success();
        }*/
    }

    public class RegistratorNotFoundError : Error
    {
        public override string Type => nameof(RegistratorNotFoundError);
    }
}
