using HyosungManagement.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using HyosungManagement.Models.Identity;
using HyosungManagement.Extensions;

namespace HyosungManagement.Data
{
    public static class UserDbInitializer
    {
        static HSMRole[] Roles
            => new[]
            {
                new HSMRole("User"),
                new HSMRole("Admin"),
                new HSMRole("Master")
            };

        static IDictionary<string, IEnumerable<PermissionGroup>> PermissionsByRole
            => new Dictionary<string, IEnumerable<PermissionGroup>>
            {
                { "User", HSMPermissionGroups.DefaultUserPermissions },
                { "Admin", HSMPermissionGroups.DefaultAdminPermissions },
                { "Master", HSMPermissionGroups.DefaultMasterPermissions },
            };

        static SecurityCode[] Codes
            => new[]
            {
                new SecurityCode
                {
                    Value = "MfbG1xuo5plab2eyCFBBbxOJDIFxKWK-ToFZwDD_0P8",
                    CodeType = SecurityCodeType.Persistent,
                    IsValid = true,
                }
            };

        public static IServiceProvider InjectDefaultUserData(this IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<UserDbContext>();
                context.Database.Migrate();
                context.AddData();

                services.GetRequiredService<RoleManager<HSMRole>>()
                        .AddRoles();
            }
            catch (Exception e)
            {
                services.GetRequiredService<ILogger<Program>>()
                        .LogError(e, "An error occured while seeding the user database with default data.");
            }

            return services;
        }

        public static UserDbContext AddData(this UserDbContext context)
        {
            if (!context.SecurityCodes.Any())
            {
                foreach (var sc in Codes)
                {
                    context.SecurityCodes.Add(sc);
                };
                context.SaveChanges();
            }

            return context;
        }

        public static RoleManager<HSMRole> AddRoles(this RoleManager<HSMRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                foreach (var role in Roles)
                {
                    roleManager.CreateAsync(role).Wait();
                    roleManager.AddRolePermissionsAsync(role, PermissionsByRole[role.Name]).Wait();
                };
            }

            return roleManager;
        }
    }
}
