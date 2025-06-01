
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
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
    public class AuditRepository : EFRepository<UserActionLog, AuditServiceDbContext>,
        IAuditRepository
    {
        private readonly AuditServiceDbContext context;

        public AuditRepository(AuditServiceDbContext context) 
            : base(context)
        {
            this.context = context;
        }
        public async Task SaveRangeAsync(List<UserActionLog> logs, CancellationToken cancellationToken)
        {
            await AddRangeAsync(logs, cancellationToken);

            await context.SaveChangesAsync();
        }
        public async Task<Result> SaveExcelActionsReport
            (ActionsReport report, CancellationToken cancellationToken)
        {
            await context.ActionsReports.AddAsync(report, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<List<ActionsReport>>> GetActionsReports
            (CancellationToken cancellationToken)
        {
            var reports = await context.ActionsReports
                .AsNoTracking()
                .ToListAsync();

            return Result<List<ActionsReport>>.Success(reports);
        }
        public async Task<Result<ActionsReport>> GetActionsReportById(Guid reportId)
        {
            var report = await context.ActionsReports.FindAsync(reportId);

            return Result<ActionsReport>.Success(report);
        }
        /*public async Result<List<UserActionLog>> GetAllByFilters()
        {
            var logs = await context.Actions
                .AsNoTracking()
                .ToListAsync();
        }*/
    }
}
