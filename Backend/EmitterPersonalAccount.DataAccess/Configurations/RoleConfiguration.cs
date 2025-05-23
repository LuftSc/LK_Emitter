using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitterPersonalAccount.Core.Domain.Enums;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public partial class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                    // Тут мы говорим, что PermissionId ссылается на PermissionEntity
                    l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(e => e.PermissionId),
                    // А тут мы говорим, что RoleId ссылается на RoleEntity
                    r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(e => e.RoleId)
                );
            // Чтобы заполнить все роли в системе

            var roles = Enum
                // Получаем все роли в системе
                .GetValues<Role>()
                // Проходимся по ним и формируем новую RoleEntity
                .Select(role => new RoleEntity
                {
                    // Id - это числовое ,представление enum'a Role
                    Id = (int)role,
                    // Имя - называние этого enum'a
                    Name = role.ToString()
                });

            // Указываем билдеру, что нужно заполнить данные:
            // Когда мы создадим миграцию, то билдер создаст скрипт,
            // который сразу заполнит таблицу ролей
            builder.HasData(roles);
        }
    }
}
