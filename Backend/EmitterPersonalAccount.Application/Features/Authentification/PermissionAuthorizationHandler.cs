using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId
            );

            if (userId == null || !Guid.TryParse(userId.Value, out var id))
            { // Если id == null или не получилось его распарсить
                return;
            }

            // Создаём scope
            using var scope = serviceScopeFactory.CreateScope();
            // Достаём нужный нам сервис 
            var permissionService = scope.ServiceProvider
                .GetRequiredService<IPermissionService>();
            // Указываем id-шник, который точно уже приведён к гуиду
            var permissions = await permissionService.GetPermissionAsync(id);

            // Если в полученных разрешениях есть хотя бы одно разрешение,
            // которое пересекается с разрешениями requirement.Permissions
            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}
