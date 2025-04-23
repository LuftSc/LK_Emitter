using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.EntityFrameworkCore.Storage;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA
{
    public class ListOSAReport : Entity<Guid>, IAggregateRoot
    {
        private ListOSAReport() : base(Guid.NewGuid())
        {
        }
        private ListOSAReport(
            Guid id,
            int issuerId,
            DateOnly dtMod,
            bool nomList,
            bool isCategMeeting,
            bool isRangeMeeting,
            DateOnly dt_Begsobr,
            ListOSAMetadata metadata
            ) : base(id)
        {
            IssuerId = issuerId;
            DtMod = dtMod;
            NomList = nomList;
            IsCategMeeting = isCategMeeting;
            IsRangeMeeting = isRangeMeeting;
            Dt_Begsobr = dt_Begsobr;
            CreatedAt = DateTime.Now.ToUniversalTime().AddHours(5);
            Metadata = metadata;
        }
        public int IssuerId { get; private set; }
        public DateOnly DtMod { get; private set; }
        public bool NomList { get; private set; }
        public bool IsCategMeeting { get; private set; }
        public bool IsRangeMeeting { get; private set; }
        public DateOnly Dt_Begsobr { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [Column(TypeName = "jsonb")]
        public ListOSAMetadata Metadata { get; private set; }

        public static Result<ListOSAReport> Create(
            Guid id,
            int issuerId,
            DateOnly dtMod,
            bool nomList,
            bool isCategMeeting,
            bool isRangeMeeting,
            DateOnly dt_Begsobr,
            ListOSAMetadata metadata
            )
        {
            return Result<ListOSAReport>
                .Success(new ListOSAReport(
                    id,
                    issuerId,
                    dtMod,
                    nomList,
                    isCategMeeting,
                    isRangeMeeting,
                    dt_Begsobr,
                    metadata));
        }
    }
}
