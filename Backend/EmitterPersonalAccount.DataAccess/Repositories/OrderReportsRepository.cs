//using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
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
    public class OrderReportsRepository 
        : EFRepository<OrderReport, EmitterPersonalAccountDbContext>, 
        IOrderReportsRepository
    {
        private readonly EmitterPersonalAccountDbContext context;

        public OrderReportsRepository(EmitterPersonalAccountDbContext context) 
            : base(context)
        {
            this.context = context;
        }

        public async Task<Result> ChangeProcessingStatusOk(Guid id, Guid externalStorageId)
        {
            await context.OrderReports
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(o => o
                    .SetProperty(p => p.Status, ReportOrderStatus.Successfull)
                    .SetProperty(p => p.ExternalStorageId,  externalStorageId));

            return Result.Success();
        }
        public async Task<Result<List<OrderReport>>> GetAllByEmitterId(Guid emitterId)
        {
            var listOrderReports = await context.OrderReports
                .Where(o => o.Emitter.Id == emitterId)
                .ToListAsync();

            return Result<List<OrderReport>>.Success(listOrderReports);
        }
        public async Task<Result> SaveAsync(Guid emitterId, OrderReport orderReport)
        {
            var emitter = await context.Emitters
                .Include(e => e.OrderReports)
                .FirstOrDefaultAsync(e => e.Id == emitterId);

            if (emitter is null) return Result
                    .Error(new EmitterNotFoundError());

            emitter.OrderReports.Add(orderReport);

            await context.OrderReports.AddAsync(orderReport);

            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
