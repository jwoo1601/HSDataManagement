using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyosungManagement.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, params string[] claimValues)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { claimValues.Select(v => new Claim(claimType, v)).ToArray() };
        }
    }

    public class RoleRequirementAttribute : ClaimRequirementAttribute
    {
        public RoleRequirementAttribute(params string[] roles)
            : base(JwtClaimTypes.Role, roles)
        {

        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly IEnumerable<Claim> requiredClaims;

        public ClaimRequirementFilter(IEnumerable<Claim> claims)
        {
            requiredClaims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.ActionDescriptor.EndpointMetadata.Any(
                    meta => typeof(IAllowAnonymous).IsAssignableFrom(meta.GetType()))
            )
            {
                var hasClaims = context.HttpContext.User.Claims.Any(
                    c => requiredClaims.Any(rc => c.Type == rc.Type && c.Value == rc.Value)
                );
                if (!hasClaims)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
