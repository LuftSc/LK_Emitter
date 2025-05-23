using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess
{
    public class AuditServiceDbContext : DbContext, IUnitOfWork
    {
        public AuditServiceDbContext(DbContextOptions<AuditServiceDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<UserEmitter> UserEmitter { get; set; }
        public DbSet<EmitterProjection> Emitters { get; set; }
        public DbSet<UserProjection> Users { get; set; }
        public DbSet<UserActionLog> Actions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserActionLog>(actionBuilder =>
            {
                actionBuilder.HasKey(a => a.Id);
            });

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(AuditServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
