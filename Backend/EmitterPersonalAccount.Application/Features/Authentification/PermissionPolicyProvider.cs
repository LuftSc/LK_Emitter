using EmitterPersonalAccount.Core.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "Permission";
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(
                new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme).
                    RequireAuthenticatedUser()
                    .Build());
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

                Enum.TryParse(policyName.Substring(POLICY_PREFIX.Length), out Permission permission);

                policy.AddRequirements(new PermissionRequirement([permission]));

                return Task.FromResult((AuthorizationPolicy?)policy.Build());
            }
            else
            {
                return Task.FromResult<AuthorizationPolicy?>(null);
            }
        }
    }
}
