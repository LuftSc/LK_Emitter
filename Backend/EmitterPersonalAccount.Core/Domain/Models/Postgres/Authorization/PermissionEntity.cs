using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Связь с ролями
        public ICollection<RoleEntity> Roles { get; set; } = [];
    }
}
