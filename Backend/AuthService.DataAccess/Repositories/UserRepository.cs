using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.DataAccess.Repositories
{
    public sealed class UserRepository 
        : EFRepository<User, AuthDbContext>, IUserRepository
    {
        public UserRepository(AuthDbContext context)
            : base(context) 
        {
            
        }
    }
}
