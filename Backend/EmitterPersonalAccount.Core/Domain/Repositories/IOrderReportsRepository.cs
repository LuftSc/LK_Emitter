using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO.ListOSA;
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
        Task<Result> ChangeProcessingStatusOk(Guid id, Guid externalStorageId);

        Task<Result> ChangeProcessingStatusFailed(Guid id);

        Task<Result> SaveAsync
            (GenerateListOSARequest listOSAReport, CancellationToken cancellationToken);
        Task<Result<List<OrderReport>>> GetAllByIssuerId(int issuerId);


        Task<Result<Tuple<int, List<OrderReport>>>> GetByPage
            (int issuerId, int page, int pageSize);


        Task<Result> SaveAsync(OrderReport orderReport, CancellationToken cancellationToken);
        
    }
}
