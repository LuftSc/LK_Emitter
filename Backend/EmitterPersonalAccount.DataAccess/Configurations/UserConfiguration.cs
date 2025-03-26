using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            /*builder.HasMany(u => u.Documents)
                .WithOne(d => d.User);*/

            builder.HasMany(u => u.Emitters)
                .WithMany(e => e.Users)
                .UsingEntity<UserEmitter>(
                    l => l.HasOne<Emitter>().WithMany().HasForeignKey(entity => entity.EmitterId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(entity => entity.UserId)
                );

            builder.HasOne(u => u.Registrator).WithMany(r => r.Users);
        }
    }
}
