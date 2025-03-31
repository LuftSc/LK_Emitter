using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalOrderReportsService.DataAccess.Configurations
{
    public class OrderReportConfiguration : IEntityTypeConfiguration<OrderReport>
    {
        public void Configure(EntityTypeBuilder<OrderReport> builder)
        {
            builder.ToTable("OrderReports").HasKey(o => o.Id);

            builder.HasOne(o => o.Emitter).WithMany(e => e.OrderReports);

            builder.HasOne(o => o.Status).WithMany().IsRequired();

            builder.Property(o => o.FileName).HasMaxLength(50).IsRequired();
            builder.Property(o => o.RequestDate).IsRequired();
        }
    }
}
