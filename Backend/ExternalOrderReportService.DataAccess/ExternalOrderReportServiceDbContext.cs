using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.DividendList;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ReeRep;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExternalOrderReportServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<OrderReport> OrderReports { get; set; }
        public DbSet<ListOSAReport> ListOSAReports { get; set; }
        public DbSet<ReeRepReport> ReeRepReports { get; set; }
        public DbSet<DividendListReport> DividendListReports { get; set; }
    }
}
