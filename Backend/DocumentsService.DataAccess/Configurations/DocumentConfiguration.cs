using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsService.DataAccess.Configurations
{
    public class DocumentConfiguration 
        : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents").HasKey(d => d.Id);
            builder.HasOne(d => d.User)
                .WithMany(u => u.Documents);
        }
    }
}
