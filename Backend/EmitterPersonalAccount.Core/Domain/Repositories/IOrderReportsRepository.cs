using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Repositories
{
    public interface IOrderReportsRepository : IRepository<OrderReport>
    {
        Task<Result> SaveAsync(Guid emitterId, OrderReport orderReport);
        Task<Result<List<OrderReport>>> GetAllByEmitterId(Guid emitterId);
        Task<Result> ChangeProcessingStatusOk(Guid id, Guid externalStorageId);
        Task<Result<Tuple<int, List<OrderReport>>>> GetByPage
            (Guid emitterId, int page, int pageSize);
    }
}
