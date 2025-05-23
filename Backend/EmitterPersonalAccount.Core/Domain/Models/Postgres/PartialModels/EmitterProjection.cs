using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels
{
    public class EmitterProjection : Entity<Guid>, IAggregateRoot
    {
        private EmitterProjection() : base(Guid.NewGuid())
        {
        }

        private EmitterProjection(Guid id, EmitterInfo emitterInfo, int issuerId) : base(id)
        {
            EmitterInfo = emitterInfo;
            IssuerId = issuerId;    
        }
        public int IssuerId { get; private set; }
        public EmitterInfo EmitterInfo { get; private set; }
        public List<UserProjection> Users { get; private set; } = [];

        public static Result<EmitterProjection> Create(
            Guid id, 
            EmitterInfo emitterInfo, 
            int issuerId)
        {
            return Result<EmitterProjection>
                .Success(new EmitterProjection(id, emitterInfo, issuerId));
        }
    }
}
