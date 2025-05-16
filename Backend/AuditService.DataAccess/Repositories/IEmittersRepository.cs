using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess.Repositories
{
    public interface IEmittersRepository : IRepository<EmitterProjection>
    {
    }
}
