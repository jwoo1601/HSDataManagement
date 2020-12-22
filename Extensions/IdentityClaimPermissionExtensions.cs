using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyosungManagement.Extensions
{
    public static class IdentityClaimPermissionExtensions
    {
        public static AuthorizationPolicyBuilder RequirePermission(
            this AuthorizationPolicyBuilder builder,
            PermissionGroup permissionGroup
        )
        {
            builder.RequireClaim(
                HSMClaimTypes.AccessPermission,
                permissionGroup.DecoratedNames
            );

            return builder;
        }

        public static async Task AddRolePermissionsAsync(
            this RoleManager<HSMRole> roleManager,
            HSMRole role,
            IEnumerable<PermissionGroup> permissions
        )
        {
            foreach (var pg in permissions)
            {
                foreach (var claim in pg.DecoratedNames)
                {
                    await roleManager.AddClaimAsync(
                        role,
                        new Claim(HSMClaimTypes.AccessPermission, claim)
                    );
                }
            }
        }
    }
}
