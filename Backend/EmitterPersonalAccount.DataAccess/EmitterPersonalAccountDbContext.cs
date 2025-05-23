using EmitterPersonalAccount.Core.Domain.Models.Configuration;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using EmitterPersonalAccount.DataAccess.Configurations;
using MassTransit.Configuration;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess
{
    public class EmitterPersonalAccountDbContext(
        DbContextOptions<EmitterPersonalAccountDbContext> options,
        IOptions<AuthorizationOptions> authOptions) : DbContext(options), 
        IUnitOfWork,
        IDataProtectionKeyContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Emitter> Emitters { get; set; }
        public DbSet<Registrator> Registrators { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEmitter> UserEmitter { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmitterPersonalAccountDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));

            base.OnModelCreating(modelBuilder);
        }
    }
}
