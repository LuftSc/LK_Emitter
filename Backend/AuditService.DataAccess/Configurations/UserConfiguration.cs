using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserProjection>
    {
        public void Configure(EntityTypeBuilder<UserProjection> builder)
        {
            builder.ToTable("Users").HasKey(u => u.Id);

            builder.HasMany(u => u.Emitters)
                .WithMany(e => e.Users)
                .UsingEntity<UserEmitter>(
                    l => l.HasOne<EmitterProjection>().WithMany().HasForeignKey(entity => entity.EmitterId),
                    r => r.HasOne<UserProjection>().WithMany().HasForeignKey(entity => entity.UserId)
                );
        }
    }
}
