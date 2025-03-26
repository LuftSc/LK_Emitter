using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsService.DataAccess
{
    public class DocumentsDbContext : DbContext, IUnitOfWork
    {
        private readonly IConfiguration configuration;

        public bool IsDisposed { get; private set; } = false;

        public DocumentsDbContext(IConfiguration configuration,
            DbContextOptions<DocumentsDbContext> options)
            :base(options)
        {
            this.configuration = configuration;
            
        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Emitter> Emitters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return base.DisposeAsync();
        }
    }
}
