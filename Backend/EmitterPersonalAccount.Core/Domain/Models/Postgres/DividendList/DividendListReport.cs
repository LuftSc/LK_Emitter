//using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.DividendList
{
    public class DividendListReport : Entity<Guid>, IAggregateRoot
    {
        private DividendListReport() : base(Guid.NewGuid())
        {
        }
        private DividendListReport(
            Guid id, 
            int issuerId, 
            DateOnly dtClo, 
            DividendListMetadata metadata): base(id) 
        {
            IssuerId = issuerId;
            DtClo = dtClo;
            CreatedAt = DateTime.Now.ToUniversalTime().AddHours(5);
            Metadata = metadata;
        }
        public int IssuerId { get; private set; } // код эмитента
        public DateOnly DtClo { get; private set; } // Дата на которую необходимо предоставить информацию
        public DateTime CreatedAt { get; private set; }

        [Column(TypeName="jsonb")]
        public DividendListMetadata Metadata { get; private set; }
        public static Result<DividendListReport> Create(
            Guid id,
            int issuerId,
            DateOnly dtClo,
            DividendListMetadata metadata
            )
        {
            return Result<DividendListReport>
                .Success(new DividendListReport(id, issuerId, dtClo, metadata));
        }
    }
}
