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
    public class EmitterConfiguration : IEntityTypeConfiguration<EmitterProjection>
    {
        public void Configure(EntityTypeBuilder<EmitterProjection> builder)
        {
            builder.ToTable("Emitters").HasKey(e => e.Id);
            // Конфигурация VO EmitterInfo
            builder.ComplexProperty(e => e.EmitterInfo, eInfoBuilder =>
            {
                eInfoBuilder.Property(p => p.FullName).HasColumnName("IO_PERS").HasMaxLength(100).IsRequired();
                eInfoBuilder.Property(p => p.ShortName).HasColumnName("F_PERS").HasMaxLength(70).IsRequired();
                eInfoBuilder.Property(p => p.INN).HasColumnName("INN").HasMaxLength(14).IsRequired();
                eInfoBuilder.Property(p => p.Jurisdiction).HasColumnName("COD_CITIZEN").HasMaxLength(15).IsRequired();

                eInfoBuilder.ComplexProperty(property => property.OGRN, ogrnBuilder =>
                {
                    ogrnBuilder.Property(p => p.Number).HasColumnName("OGRN").HasMaxLength(20);
                    ogrnBuilder.Property(p => p.DateOfAssignment).HasColumnName("DT_OGRN");
                    ogrnBuilder.Property(p => p.Issuer).HasColumnName("GIV_OGRN").HasMaxLength(60);
                });

                eInfoBuilder.ComplexProperty(property => property.Registration, regBuilder =>
                {
                    regBuilder.Property(p => p.Number).HasColumnName("S_N").HasMaxLength(20).IsRequired(false);
                    regBuilder.Property(p => p.RegistrationDate).HasColumnName("DT_DOC").HasDefaultValue(DateOnly.MinValue);
                    regBuilder.Property(p => p.Issuer).HasColumnName("GIVEN").HasMaxLength(60).IsRequired(false);
                });
            });
        }
    }
}
