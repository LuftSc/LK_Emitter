using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Какая логика?
        // Есть пользователь, у него есть роль(~админ)
        // У админа есть набор пермиссий(набор разрешений)
        // Permissions нужны, чтобы сделать систему авторизации более гибкой
        public ICollection<PermissionEntity> Permissions { get; set; } = [];
        //Связь с юзерами многие ко многим
        public ICollection<User> Users { get; set; } = [];
    }
}
