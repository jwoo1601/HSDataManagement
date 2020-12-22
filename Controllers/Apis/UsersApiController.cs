using HyosungManagement.Data;
using HyosungManagement.Filters;
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
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : ApiControllerBase
    {
        UserDbContext Context { get; }
        ILogger<HSMUser> Logger { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        IStringLocalizer<UsersApiController> Localizer { get; }

        public UsersApiController(
            UserDbContext context,
            ILogger<HSMUser> logger,
            RoleManager<IdentityRole> roleManager,
            IStringLocalizer<UsersApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            RoleManager = roleManager;
            Localizer = localizer;
        }

        // GET /api/users
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var list = await Context.Users
                                            .ToListAsync();

            return Ok(list);
        }


        // GET /api/users/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByIDAsync(
            string id
        )
        {
            var user = await Context.Users
                                            .SingleOrDefaultAsync(u => u.Id.Equals(id));

            return Ok(user);
        }


        // GET /api/users/{id}/inactivate
        [HttpGet("{id}/inactivate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InactivateUserAsync(
            string id
        )
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
                if (user == null)
                {
                    return NotFound();
                }

                user.IsActive = false;
                Context.Update(user);
                await Context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException e)
            {
                return ServerError("Failed to inactivate user");
            }
        }
    }
}
