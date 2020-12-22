using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyosungManagement.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(JwtClaimTypes.PreferredUserName);
        }

        public static string GetUsersName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(JwtClaimTypes.Name);
        }

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(JwtClaimTypes.Role)?.Split(" ") ?? Enumerable.Empty<string>();
        }

        public static string GetFirstInRoles(this ClaimsPrincipal user)
        {
            return user.GetRoles().FirstOrDefault();
        }

        public static string GetUserID(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(JwtClaimTypes.Subject);
        }
    }
}
