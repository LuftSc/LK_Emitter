using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExternalOrderReportService.DataAccess.Configurations
{
    public class ListOSAReportsConfiguration : IEntityTypeConfiguration<ListOSAReport>
    {
        public void Configure(EntityTypeBuilder<ListOSAReport> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<ListOSAMetadata>(v, JsonSerializerOptions.Default));
        }
    }
}
