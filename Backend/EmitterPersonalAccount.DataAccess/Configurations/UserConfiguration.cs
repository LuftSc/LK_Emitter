using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
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
                .WithOne(d => d.User);
            public string EncryptedEmail { get; private set; } = string.Empty;
        public string EncryptedPhone { get; private set; } = string.Empty;
        public string EncryptedBirthDate { get; private set; } = string.Empty;
        public EncryptedPassport EncryptedPassport { get; private set; } = EncryptedPassport.Empty;
        public string EncryptedFullName { get; private set; } = string.Empty;
        public string FullNameSearchHash { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
             
             */

            builder.ComplexProperty(p => p.EncryptedPassport, passportBuilder =>
            {
                passportBuilder.Property(f => f.Issuer).HasColumnName("PassportIssuer").IsRequired(false);
                passportBuilder.Property(f => f.Series).HasColumnName("Series").IsRequired(false);
                passportBuilder.Property(f => f.UnitCode).HasColumnName("UnitCode").IsRequired(false);
                passportBuilder.Property(f => f.DateOfIssue).HasColumnName("DateOfIssue").IsRequired(false);
                passportBuilder.Property(f => f.Number).HasColumnName("Number").IsRequired(false);
            });

            builder.HasMany(u => u.Emitters)
                .WithMany(e => e.Users)
                .UsingEntity<UserEmitter>(
                    l => l.HasOne<Emitter>().WithMany().HasForeignKey(entity => entity.EmitterId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(entity => entity.UserId)
                );

            //builder.HasOne(u => u.Registrator).WithMany(r => r.Users);

            builder.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity<UserRole>(
                left => left.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                right => right.HasOne<User>().WithMany().HasForeignKey(u => u.UserId)
            );
        
        }
    }
}
