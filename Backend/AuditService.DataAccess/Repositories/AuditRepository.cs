using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.DataAccess.Repositories
{
    public class AuditRepository : EFRepository<UserActionLog, AuditServiceDbContext>,
        IAuditRepository
    {
        private readonly AuditServiceDbContext context;

        public AuditRepository(AuditServiceDbContext context) 
            : base(context)
        {
            this.context = context;
        }
        public async Task SaveRangeAsync(List<UserActionLog> logs, CancellationToken cancellationToken)
        {
            await AddRangeAsync(logs, cancellationToken);

            await context.SaveChangesAsync();
        }
    }
}
