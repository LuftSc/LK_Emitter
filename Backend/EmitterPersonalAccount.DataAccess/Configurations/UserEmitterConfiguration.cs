using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public class UserEmitterConfiguration : IEntityTypeConfiguration<UserEmitter>
    {
        public void Configure(EntityTypeBuilder<UserEmitter> builder)
        {
            builder.HasKey(entity => new { entity.UserId, entity.EmitterId });
        }
    }
}
