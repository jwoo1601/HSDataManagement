using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Master")]
    [Route("api/roles")]
    [ApiController]
    public class RolesApiController : ApiControllerBase
    {
        UserDbContext Context { get; }
        ILogger<HSMRole> Logger { get; }
        IStringLocalizer<RolesApiController> Localizer { get; }

        public RolesApiController(
            UserDbContext context,
            ILogger<HSMRole> logger,
            IStringLocalizer<RolesApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }
    }
}
