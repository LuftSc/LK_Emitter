using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
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
    public sealed class EmittersRepository 
        : EFRepository<Emitter, EmitterPersonalAccountDbContext>, IEmittersRepository
    {
        private readonly EmitterPersonalAccountDbContext context;

        public EmittersRepository(EmitterPersonalAccountDbContext context) 
            : base(context)
        {
            this.context = context;
        }

        public async Task<Result> BindUser(Guid emitterId, Guid userId)
        {
            var user = await context.Users
                .Include(u => u.Emitters)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return Result.Error(new UserNotFoundError());

            var emitter = await context.Emitters
                .Include(e => e.Registrator)
                .FirstOrDefaultAsync(x => x.Id == emitterId);

            if (emitter is null)
                return Result.Error(new EmitterNotFoundError());

            if (user.Emitters.Contains(emitter))
                return Result.Error(new UserAlreadyBoundedError());

            user.Registrator = emitter.Registrator;

            user.Emitters.Add(emitter);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<List<Tuple<Guid, EmitterInfo>>>> GetAllByUserId(Guid userId)
        {
            var user = await context.Users
                .Include(u => u.Emitters)
                .Include(u => u.Registrator)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return Result<List<Tuple<Guid, EmitterInfo>>>
                    .Error(new UserNotFoundError());

            var emittersInfo = new List<Tuple<Guid, EmitterInfo>>();

            // Пользователь - сотрудник регистратора
            Console.WriteLine(user.Registrator);
            if (user.Registrator is null)
            {
                emittersInfo = context.Emitters
                    .Select(e => Tuple.Create(e.Id, e.EmitterInfo))
                    .ToList();
            }
            else // Пользователь - представитель эмитента
            {
                emittersInfo = user.Emitters
                    .Select(e => Tuple.Create(e.Id, e.EmitterInfo))
                    .ToList();
            }

            return Result<List<Tuple<Guid, EmitterInfo>>>.Success(emittersInfo);
        }
    }

    public class EmitterNotFoundError : Error
    {
        public override string Type => nameof(EmitterNotFoundError);
    }
    public class UserNotFoundError : Error
    {
        public override string Type => nameof(UserNotFoundError);
    }
    public class UserAlreadyBoundedError : Error
    {
        public override string Type => nameof(UserAlreadyBoundedError);
    }
}
