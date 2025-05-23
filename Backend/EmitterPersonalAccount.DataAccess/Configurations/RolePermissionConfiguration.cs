using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitterPersonalAccount.Core.Domain.Models.Configuration;
using EmitterPersonalAccount.Core.Domain.Enums;

namespace EmitterPersonalAccount.DataAccess.Configurations
{
    public partial class RolePermissionConfiguration
        : IEntityTypeConfiguration<RolePermission>
    // Эта конфигурация нужна для того, чтобы сразу в системе задать:
    // - Для роли админа -> такие-то пермиссии
    // - Для роли юзера -> другие пермиссии
    {
        private readonly AuthorizationOptions authorizationOption;

        public RolePermissionConfiguration(AuthorizationOptions authorizationOption)
        {
            this.authorizationOption = authorizationOption;
        }
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Назначаем составной ключ
            builder.HasKey(role => new { role.RoleId, role.PermissionId });

            builder.HasData(ParseRolePermissions());
        }
        private RolePermission[] ParseRolePermissions()
        {
            return authorizationOption.RolePermissions 
                .SelectMany(rp => rp.Permissions 
                    .Select(p => new RolePermission
                        {
                            RoleId = (int)Enum.Parse<Role>(rp.Role),
                            PermissionId = (int)Enum.Parse<Permission>(p)
                        }))
                    .ToArray();
        }
    }
}
