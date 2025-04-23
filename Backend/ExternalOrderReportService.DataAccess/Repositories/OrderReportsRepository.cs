using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.DividendList;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ReeRep;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalOrderReportService.DataAccess.Repositories
{
    public class OrderReportsRepository : 
        EFRepository<OrderReport, ExternalOrderReportServiceDbContext>, 
        IOrderReportsRepository
    {
        private readonly ExternalOrderReportServiceDbContext context;

        public OrderReportsRepository(ExternalOrderReportServiceDbContext context) 
            : base(context)
        {
            this.context = context;
        }

        public async Task<Result> ChangeProcessingStatusFailed(Guid id)
        {
            await context.OrderReports
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(o => o
                    .SetProperty(p => p.Status, CompletionStatus.Failed));

            return Result.Success();
        }
        public async Task<Result> ChangeProcessingStatusOk(Guid id, Guid externalStorageId)
        {
            await context.OrderReports
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(o => o
                    .SetProperty(p => p.Status, CompletionStatus.Successfull)
                    .SetProperty(p => p.ExternalStorageId, externalStorageId));

            return Result.Success();
        }

        public async Task<Result<List<OrderReport>>> GetAllByIssuerId(int issuerId)
        {
            var listOrderReports = await context.OrderReports.AsNoTracking()
                .Where(o => o.IssuerId == issuerId)
                .ToListAsync();

            return Result<List<OrderReport>>.Success(listOrderReports);
        }

        public async Task<Result<Tuple<int, List<OrderReport>>>> GetByPage
            (int issuerId, int page, int pageSize)
        {
            var query = context.OrderReports
                .AsNoTracking()
                .AsQueryable()
                .Where(o => o.IssuerId == issuerId)
                .OrderByDescending(o => o.RequestDate);

            var totalSize = await query.CountAsync();

            var listOrderReports = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Result<Tuple<int, List<OrderReport>>>
                .Success(Tuple.Create(totalSize, listOrderReports));
        }

        public async Task<Result> SaveAsync(OrderReport orderReport, CancellationToken cancellationToken)
        {
            await AddAsync(orderReport, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> SaveAsync
            (GenerateListOSARequest listOSAReport, CancellationToken cancellationToken)
        {
            var reportCreatingResult = ListOSAReport.Create(
                Guid.Parse(listOSAReport.InternalDocumentId),
                listOSAReport.IssuerId,
                DateOnly.Parse(listOSAReport.DtMod),
                listOSAReport.NomList,
                listOSAReport.IsCategMeeting,
                listOSAReport.IsRangeMeeting,
                DateOnly.Parse(listOSAReport.Dt_Begsobr),
                listOSAReport.ExtractMetadata()
            );

            if (!reportCreatingResult.IsSuccessfull) return reportCreatingResult;

            await context.ListOSAReports.AddAsync(reportCreatingResult.Value, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> SaveAsync(GenerateReeRepRequest reeReport, 
            CancellationToken cancellationToken)
        {
            var reportCreatingResult = ReeRepReport.Create(
                Guid.Parse(reeReport.InternalDocumentId), 
                reeReport.EmitId, 
                reeReport.ProcUk, 
                reeReport.NomList, 
                reeReport.DtMod,
                reeReport.OneProcMode,
                reeReport.ExtractMetadata());

            if (!reportCreatingResult.IsSuccessfull) return reportCreatingResult;

            await context.ReeRepReports.AddAsync(reportCreatingResult.Value, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> SaveAsync(GenerateDividendListRequest divListReport, 
            CancellationToken cancellationToken)
        {
            var reportCreatingResult = DividendListReport.Create(
                Guid.Parse(divListReport.InternalDocumentId),
                divListReport.IssuerId,
                divListReport.DtClo,
                divListReport.ExtractMetadata());

            if (!reportCreatingResult.IsSuccessfull) return reportCreatingResult;

            await context.DividendListReports.AddAsync(reportCreatingResult.Value, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

