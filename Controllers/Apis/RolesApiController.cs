using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.Logging;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Mime;
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

        // GET /api/roles
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var list = await Context.Roles.ToListAsync();

            Logger.LogInformation(
                RolesApiLogEvents.GetAllRoles,
                "Successfully retrieved roles - num: {num}.",
                list.Count
            );

            return Ok(list);
        }


        // GET /api/roles/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoleByIDAsync(
            string id
        )
        {
            var role = await Context.Roles.SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (role == null)
            {
                Logger.LogError(
                    RolesApiLogEvents.RoleNotFound,
                    "Failed to retrieve role {ID}: role not found.",
                    id
                );

                return NotFound(Localizer["role.notFound"]);
            }

            Logger.LogInformation(
                RolesApiLogEvents.GetRoleByID,
                "Successfully retrieved role {@Role}",
                role
            );

            return Ok(role);
        }
    }
}
