using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Repositories
{
    public class RegistratorRepository 
        : EFRepository<Registrator, EmitterPersonalAccountDbContext>, 
        IRegistratorRepository
    {
        public RegistratorRepository(EmitterPersonalAccountDbContext context) : base(context)
        {
        }
    }
}
