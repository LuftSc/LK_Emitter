using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Repositories
{
    public interface IEmittersRepository : IRepository<Emitter>
    {
        Task<List<Tuple<Guid, EmitterInfo, int>>> SearchEmitter(string searchTerm, int page = 1, int pageSize = 20);
        //Task<Result> BindUser(Guid emitterId, Guid userId);
        Task<Result<List<Tuple<Guid, EmitterInfo, int>>>> GetAllByUserId(Guid userId);
        Task<Result<List<Tuple<Guid, EmitterInfo>>>> GetProjections();
    }
}
