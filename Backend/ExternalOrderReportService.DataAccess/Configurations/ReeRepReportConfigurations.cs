//using Microsoft.EntityFrameworkCore.Metadata;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.ReeRep;
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
    public class ReeRepReportConfigurations : IEntityTypeConfiguration<ReeRepReport>
    {
        public void Configure(EntityTypeBuilder<ReeRepReport> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<ReeRepMetadata>(v, JsonSerializerOptions.Default));
        }
    }
}
