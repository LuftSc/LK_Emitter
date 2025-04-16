using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalOrderReportService.DataAccess
{
    public class ExternalOrderReportServiceDbContext : DbContext, IUnitOfWork
    {
        public ExternalOrderReportServiceDbContext
            (DbContextOptions<ExternalOrderReportServiceDbContext> options)
            : base (options)
        {
            
        }
        public DbSet<OrderReport> OrderReports { get; set; }
    }
}
