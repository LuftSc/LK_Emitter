using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public class RegistratorConfiguration : IEntityTypeConfiguration<Registrator>
    {
        public void Configure(EntityTypeBuilder<Registrator> builder)
        {
            builder.ToTable("Registrator").HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("REGISTRATOR_ID");

            builder.HasMany(r => r.Users).WithOne(u => u.Registrator);
            builder.HasMany(r => r.Emitters).WithOne(e => e.Registrator);

            builder.ComplexProperty(property => property.OGRN, ogrnBuilder =>
            {
                ogrnBuilder.Property(p => p.Number).HasColumnName("OGRN").HasMaxLength(20);
                ogrnBuilder.Property(p => p.DateOfAssignment).HasColumnName("DT_OGRN");
                ogrnBuilder.Property(p => p.Issuer).HasColumnName("GIV_OGRN").HasMaxLength(60);
            });

            builder.ComplexProperty(m => m.Location, locBuilder =>
            {
                locBuilder.Property(p => p.Country).HasColumnName("COD_COUNTRY").HasMaxLength(15);
                locBuilder.Property(p => p.Index).HasColumnName("IND");
                locBuilder.Property(p => p.Region).HasColumnName("COD_REGION").HasMaxLength(25);
                // Конфигурация для VO Address
                locBuilder.Property(p => p.Address).HasColumnName("ADDRESS").HasConversion(
                    a => a.ToString(), // Когда ложим объект в БД
                    a => Address.Parse(a) // Когда достаём из БД
                    ).HasMaxLength(110);
            });
        }
    }
}
