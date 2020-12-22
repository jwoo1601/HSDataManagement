using Hangfire.Annotations;
using Hangfire.Dashboard;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var hostEnvironment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            if (hostEnvironment.IsDevelopment())
            {
                return true;
            }

            if (
                httpContext.User.HasClaim(
                    HSMClaimTypes.AccessPermission,
                    HSMPermissionGroups.NewPermissionGroup(
                        HSMPermissionSections.Hangfire,
                        PermissionTypes.Manage
                    ).DecoratedNames.First()
                )
            )
            {
                return true;
            }

            return false;
        }
    }
}
