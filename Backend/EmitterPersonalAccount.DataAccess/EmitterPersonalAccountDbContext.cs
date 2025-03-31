using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess
{
    public class EmitterPersonalAccountDbContext : DbContext, IUnitOfWork
    {
        public EmitterPersonalAccountDbContext
            (DbContextOptions<EmitterPersonalAccountDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Emitter> Emitters { get; set; }
        public DbSet<Registrator> Registrators { get; set; }
        public DbSet<OrderReport> OrderReports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmitterPersonalAccountDbContext).Assembly);

           /* modelBuilder.Entity<ReportOrderStatus>(statusBuilder =>
            {
                statusBuilder.HasData(
                    ReportOrderStatus.Successfull,
                    ReportOrderStatus.Processing,
                    ReportOrderStatus.Failed);
            });*/
            base.OnModelCreating(modelBuilder);
        }
    }
}
