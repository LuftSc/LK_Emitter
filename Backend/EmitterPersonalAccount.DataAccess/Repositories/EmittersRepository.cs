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

        public async Task<List<Tuple<Guid, EmitterInfo, int>>> SearchEmitter(string searchTerm, int page = 1, int pageSize = 20)
        {
            var query = context.Emitters
                .Where(e => EF.Functions.ILike(e.EmitterInfo.ShortName, $"%{searchTerm}%"))
                .OrderBy(e => e.EmitterInfo.ShortName)
                .Select(e => Tuple.Create(e.Id, e.EmitterInfo, e.IssuerId));

            var results = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return results;
        }

        public async Task<Result<List<Tuple<Guid, EmitterInfo>>>> GetProjections()
        {
            var emittersProjections = new List<Tuple<Guid, EmitterInfo>>();

            emittersProjections = await context.Emitters
                .Select(e => Tuple.Create(e.Id, e.EmitterInfo))
                .ToListAsync();

            return Result<List<Tuple<Guid, EmitterInfo>>>.Success(emittersProjections);
        }

       /*public async Task<Result> BindUser(Guid emitterId, Guid userId)
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
        }*/
        public async Task<Result<List<Tuple<Guid, EmitterInfo, int>>>> GetAllByUserId(Guid userId)
        {
            var user = await context.Users
                .Include(u => u.Emitters)
                .Include(u => u.Registrator)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return Result<List<Tuple<Guid, EmitterInfo, int>>>
                    .Error(new UserNotFoundError());

            var emittersInfo = new List<Tuple<Guid, EmitterInfo, int>>();

            // Пользователь - сотрудник регистратора
            Console.WriteLine(user.Registrator);
            if (user.Registrator is null)
            {
                emittersInfo = context.Emitters
                    .Select(e => Tuple.Create(e.Id, e.EmitterInfo, e.IssuerId))
                    .ToList();
            }
            else // Пользователь - представитель эмитента
            {
                emittersInfo = user.Emitters
                    .Select(e => Tuple.Create(e.Id, e.EmitterInfo, e.IssuerId))
                    .ToList();
            }

            return Result<List<Tuple<Guid, EmitterInfo, int>>>.Success(emittersInfo);
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
