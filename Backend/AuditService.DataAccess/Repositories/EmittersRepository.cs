using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess.Repositories
{
    public class EmittersRepository 
        : EFRepository<EmitterProjection, AuditServiceDbContext>, 
        IEmittersRepository
    {
        private readonly AuditServiceDbContext context;

        public EmittersRepository(AuditServiceDbContext context) : base(context)
        {
            this.context = context;
        }


    }
}
