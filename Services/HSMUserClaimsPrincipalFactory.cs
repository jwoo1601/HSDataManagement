using HyosungManagement.Models.Identity;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public class HSMUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<HSMUser>
    {
        public HSMUserClaimsPrincipalFactory(
            UserManager<HSMUser> userManager,
            IOptions<IdentityOptions> optionsAccessor
        ) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(HSMUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(JwtClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));
            identity.AddClaim(new Claim(JwtClaimTypes.Role, string.Join(" ", user.Roles)));

            return identity;
        }
    }
}
