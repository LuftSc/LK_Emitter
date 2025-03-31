using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalOrderReportsService.DataAccess.Repositories
{
    public class OrderReportsRepository
        : EFRepository<OrderReport, ExternalOrderReportsServiceDbContext>,
        IOrderReportsRepository
    {
        private readonly ExternalOrderReportsServiceDbContext context;

        public OrderReportsRepository(ExternalOrderReportsServiceDbContext context) 
            : base(context)
        {
            this.context = context;
        }

    }
}
