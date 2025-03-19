using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using Registrator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registrator.DataAccess
{
    public class RegistratorDbContext : DbContext, IUnitOfWork
    {
        public RegistratorDbContext(DbContextOptions<RegistratorDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Directive>(directiveBuilder =>
            {
                directiveBuilder.HasKey(p => p.Id);
                directiveBuilder.Property(p => p.MIMEType).HasMaxLength(15);
                directiveBuilder.Property(p => p.FileName).HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Directive> Directives { get; set; }
    }
}
