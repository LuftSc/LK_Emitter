using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.ReeRep
{
    public class ReeRepReport : Entity<Guid>, IAggregateRoot
    {
        private ReeRepReport() : base(Guid.NewGuid())
        {
        }
        private ReeRepReport(
            Guid id, 
            int emitId, 
            int procUk, 
            bool nomList, 
            DateOnly dtMod,
            bool oneProcMode,
            ReeRepMetadata metadata) : base(id)
        {
            IssuerId = emitId;
            ProcUk = procUk;
            NomList = nomList;
            DtMod = dtMod;
            CreatedAt = DateTime.Now.ToUniversalTime().AddHours(5);
            OneProcMode = oneProcMode;
            Metadata = metadata;
        }
        public int IssuerId { get; private set; }
        public int ProcUk { get; private set; } 
        public bool NomList { get; private set; }
        public DateOnly DtMod { get; private set; }
        public bool OneProcMode { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [Column(TypeName = "jsonb")]
        public ReeRepMetadata Metadata { get; private set; }
        public static Result<ReeRepReport> Create(
            Guid id,
            int emitId,
            int procUk,
            bool nomList,
            DateOnly dtMod,
            bool oneProcMode,
            ReeRepMetadata metadata)
        {
            return Result<ReeRepReport>
                .Success(new ReeRepReport(id, emitId, procUk, nomList, dtMod, oneProcMode, metadata));
        }
    }
}
