using HyosungManagement.Configurations;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using HyosungManagement.Extensions;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityModel;

namespace HyosungManagement.Data
{
    public static class AuthDbInitializer
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

        static IdentityResource[] IdentityResources
            => new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = HSMApiScopes.OpenId.Profile,
                    DisplayName = "User's profile",
                    UserClaims = {
                        "name",
                        "username",
                        "email",
                        "email_verified",
                        "role",
                        "locale",
                        "updated_at"
                    }
                },
                new IdentityResource
                {
                    Name = "role",
                    DisplayName = "User's role",
                    UserClaims = { "role" }
                },
                new IdentityResource
                {
                    Name = "security_code",
                    DisplayName = "User's security code at registration",
                    UserClaims = { "security_code" }
                }
            };

        static IEnumerable<ApiScope> ApiScopes
            => HSMApiScopes
                .ListScopes(HSMApiScopeType.Identity)
                .Concat(
                    HSMApiScopes.ListScopes(HSMApiScopeType.Main)
                )
                .Select(s => new ApiScope(s))
                .Append(new ApiScope(
                    "HSMApi",
                    new[] {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.PreferredUserName,
                        JwtClaimTypes.Role
                    }
                ));

        static IEnumerable<ApiResource> Apis => new List<ApiResource>
        {
            new ApiResource("HSMApi")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Role
                }
            }
        };

        static Client[] Clients
            => new[]
            {
                new Client
                {
                    ClientId = "hsm-oidc",
                    ClientName = "Hyosung Management OpenID Connect Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = HSMApiScopes.ListScopes<HSMApiScopes.OpenId>().ToArray()
                },
                new Client
                {
                    ClientId = "hsm-interactive",
                    ClientName = "Hyosung Management Interactive Web Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = HSMApiScopes
                                    .ListScopes<HSMApiScopes.OpenId>()
                                    .Concat(
                                        HSMApiScopes.ListScopes(HSMApiScopeType.Main)
                                    ).Append(
                                        "HSMApi"
                                    ).ToArray(),
                    RequireClientSecret = false,
                    //RequirePkce = true,

                    //RedirectUris = { "http://localhost:5000/" },
                    //PostLogoutRedirectUris = { "http://localhost:5000/" },
                    AllowedCorsOrigins = {
                        "http://localhost:5000",
                        "http://localhost:8080"
                    },

                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientId = "hsm-client-credentials",
                    ClientName = "Hyosung Management Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        "client"
                    }
                }
            };

        public static IServiceProvider AddDefaultAuthData(this IServiceProvider services)
        {
            try
            {
                services.GetRequiredService<RoleManager<HSMRole>>()
                        .AddRoles();

                services.GetRequiredService<AuthDbContext>()
                        .InitAuthData();
            }
            catch (Exception e)
            {
                services.GetRequiredService<ILogger<Program>>()
                        .LogError(e, "An error occured while seeding the database with auth data.");
            }

            return services;
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

        public static AuthDbContext InitAuthData(this AuthDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.IdentityResources.Any())
            {
                foreach (var ir in IdentityResources)
                {
                    context.IdentityResources.Add(ir.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var s in ApiScopes)
                {
                    context.ApiScopes.Add(s.ToEntity());
                }
                context.SaveChanges();
            }


            if (!context.ApiResources.Any())
            {
                foreach (var res in Apis)
                {
                    context.ApiResources.Add(res.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                foreach (var c in Clients)
                {
                    context.Clients.Add(c.ToEntity());
                }
                context.SaveChanges();
            }

            return context;
        }
    }
}
