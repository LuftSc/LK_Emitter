using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess.Repositories
{
    public class UsersRepository : EFRepository<UserProjection, AuditServiceDbContext>, IUsersRepository
    {
        private readonly AuditServiceDbContext context;

        public UsersRepository(AuditServiceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Result> AddUserWithEmittersBindings
            (UserProjection user, List<Guid> emittersId, CancellationToken cancellation)
        {
            foreach (var emitterId in emittersId)
            {
                var emitter = await context.Emitters.FindAsync(emitterId);

                if (emitter is null) return Result.Error(new EmitterNotFoundError());

                user.Emitters.Add(emitter);
            }

            await AddAsync(user, cancellation);
            await context.SaveChangesAsync(cancellation);

            return Result.Success();
        }

        public async Task<Result> BindToEmitters(List<Guid> emittersListId, Guid userId, CancellationToken cancellation)
        {
            foreach (var emitterId in emittersListId)
            {
                var entry = await context.UserEmitter
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EmitterId == emitterId, cancellation);

                if (entry is null)
                {
                    var bindingCreatingResult = UserEmitter.Create(userId, emitterId);
                    if (bindingCreatingResult.IsSuccessfull)
                    {
                        await context.UserEmitter.AddAsync(bindingCreatingResult.Value, cancellation);
                    }
                }
            }

            await context.SaveChangesAsync(cancellation);

            return Result.Success();
        }
    }

    public class EmitterNotFoundError : Error
    {
        public override string Type => nameof(EmitterNotFoundError);
    }
}
