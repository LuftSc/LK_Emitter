using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using ExternalOrderReportsService.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExternalOrderReportsService.DataAccess
{
    public class ExternalOrderReportsServiceDbContext : DbContext, IUnitOfWork
    {
        public ExternalOrderReportsServiceDbContext
            (DbContextOptions<ExternalOrderReportsServiceDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<OrderReport> OrderReports { get; set; }
        public DbSet<Emitter> Emitters { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(ExternalOrderReportsServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
