using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public class EmitterConfiguration : IEntityTypeConfiguration<Emitter>
    {
        public void Configure(EntityTypeBuilder<Emitter> builder)
        {
            builder.ToTable("Emitters").HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("EMIT_ID");

            builder.HasOne(e => e.Registrator).WithMany(r => r.Emitters);
            builder.HasMany(e => e.Documents)
                .WithOne(d => d.Emitter)
                .OnDelete(DeleteBehavior.Cascade);

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

            // Конфигурация VO Location
            builder.ComplexProperty(e => e.Location, locBuilder =>
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
            // Конфигурация VO MailingAddress
            builder.ComplexProperty(e => e.MailingAddress, mailBuilder =>
            {
                mailBuilder.ComplexProperty(m => m.Location, locBuilder =>
                {
                    locBuilder.Property(p => p.Country).HasColumnName("COD_PCOUNTRY").HasMaxLength(15);
                    locBuilder.Property(p => p.Index).HasColumnName("PIND");
                    locBuilder.Property(p => p.Region).HasColumnName("COD_PREGION").HasMaxLength(25);
                    // Конфигурация для почтового VO Address
                    locBuilder.Property(p => p.Address).HasColumnName("PADDRESS").HasConversion(
                        a => a.ToString(), // Когда ложим объект в БД
                        a => Address.Parse(a) // Когда достаём из БД
                        ).HasMaxLength(110);
                });
                // Конфигурация VO Contacts
                mailBuilder.Property(p => p.Contacts)
                    .HasColumnName("ContactInfo")
                    .HasConversion(
                        contacts => contacts.ToXml(),
                        contacts => Contacts.FromXml(contacts)
                    ).IsRequired(false);
            });

            builder.Property(e => e.IsPersonalDocumentsReception).HasColumnName("ONLY_PERS");
            builder.Property(e => e.AuthorizedCapital).HasColumnName("UK");
            builder.Property(e => e.IsInformationDisclosure).HasColumnName("PUBLIC_INFO");
            builder.Property(e => e.MethodGettingInfoFromRegistry).HasColumnName("POST_INF");
            builder.Property(e => e.MeetNotifyXML).HasColumnName("MEET_NOTIFY").IsRequired(false);
            // Конфигурация VO BankDetails
            builder.ComplexProperty(property => property.BankDetails, bankBuilder =>
            {
                bankBuilder.Property(p => p.BIK).HasColumnName("BIC").HasMaxLength(10);
                bankBuilder.Property(p => p.BankName).HasColumnName("BNAME");
                bankBuilder.Property(p => p.SettlementAccount).HasColumnName("BR_SCH").HasMaxLength(10).IsRequired(false);
                bankBuilder.Property(p => p.CorrespondentAccount).HasColumnName("BCOR_SCH");
                bankBuilder.Property(p => p.BankINN).HasColumnName("BINN").IsRequired(false);
                bankBuilder.Property(p => p.Department).HasColumnName("BDEPART").IsRequired(false);
                bankBuilder.Property(p => p.CustomerAccount).HasColumnName("R_S").HasMaxLength(21);
                // не факт
                bankBuilder.Property(p => p.TaxBenefits).HasColumnName("TYP_PERS");
                bankBuilder.Property(p => p.Country).HasColumnName("FOREIGN_BCOUNTRY").IsRequired(false);
            });
            // Конфигурация VO PaymentRecipient
            builder.ComplexProperty(property => property.PaymentRecipient, recBuilder =>
            {
                recBuilder.Property(p => p.Name).HasColumnName("RECNAME").IsRequired(false);
                recBuilder.Property(p => p.INN).HasColumnName("RECINN").HasMaxLength(14).IsRequired(false);
                recBuilder.Property(p => p.Assignment).HasColumnName("RECGIVEN").HasMaxLength(60).IsRequired(false);
            });

            builder.Property(p => p.FieldOfActivity).HasColumnName("TYP_KLS").IsRequired();
            builder.Property(p => p.AdditionalInformation).HasColumnName("INFO").IsRequired(false);
        }
    }
}
