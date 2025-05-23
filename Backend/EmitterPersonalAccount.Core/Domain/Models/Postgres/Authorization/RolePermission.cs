using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
