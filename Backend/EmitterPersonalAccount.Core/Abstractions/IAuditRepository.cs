using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IAuditRepository : IRepository<UserActionLog>
    {
        Task SaveRangeAsync(List<UserActionLog> logs, CancellationToken cancellationToken);
        Task<Result> SaveExcelActionsReport
            (ActionsReport report, CancellationToken cancellationToken);
        Task<Result<List<ActionsReport>>> GetActionsReports
            (CancellationToken cancellationToken);
        Task<Result<ActionsReport>> GetActionsReportById(Guid reportId);
    }
}
