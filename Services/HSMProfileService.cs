using HyosungManagement.Models.Identity;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public class HSMProfileService : IProfileService
    {
        UserManager<HSMUser> UserManager { get; }

        public HSMProfileService(
            UserManager<HSMUser> userManager
        )
        {
            UserManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var currentUser = await UserManager.GetUserAsync(context.Subject);
            var additionalClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, currentUser.Name),
                new Claim("username", currentUser.UserName),
                new Claim(JwtClaimTypes.PreferredUserName, currentUser.UserName),
                new Claim(JwtClaimTypes.Email, currentUser.Email),
                new Claim(JwtClaimTypes.EmailVerified, currentUser.EmailConfirmed.ToString().ToLowerInvariant()),
                new Claim(JwtClaimTypes.Role, string.Join(" ", currentUser.Roles.Select(a => a.Role.Name))),
                new Claim(JwtClaimTypes.Locale, currentUser.Locale),
                new Claim("is_active", currentUser.IsActive.ToString().ToLowerInvariant()),
                new Claim("registered_at", currentUser.RegisteredAt.ToString("yyyy-MM-ddTHH:mm:ss")),
                new Claim(JwtClaimTypes.UpdatedAt, currentUser.LastUpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss")),
                new Claim("access_failed_count", currentUser.AccessFailedCount.ToString())
            };

            //context.IssuedClaims.Clear();
            context.AddRequestedClaims(additionalClaims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var currentUser = await UserManager.GetUserAsync(context.Subject);
            context.IsActive = currentUser?.IsActive ?? false;
        }
    }
}
