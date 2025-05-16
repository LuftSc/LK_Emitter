using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
    }
}
