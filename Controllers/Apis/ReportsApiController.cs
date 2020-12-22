using HyosungManagement.Data;
using HyosungManagement.Extensions;
using HyosungManagement.Filters;
using HyosungManagement.Models;
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
    [Route("api/Reports")]
    [ApiController]
    public class ReportsApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Report> Logger { get; }
        IStringLocalizer<ReportsApiController> Localizer { get; }
        UserManager<HSMUser> UserManager { get; }

        public ReportsApiController(
            AppDbContext context,
            ILogger<Report> logger,
            IStringLocalizer<ReportsApiController> localizer,
            UserManager<HSMUser> userManager
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
            UserManager = userManager;
        }

        // GET /api/reports
        [RoleRequirement("Master")]
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReportsAsync()
        {
            var list = await Context.Reports
                                            .ToListAsync();

            return Ok(list);
        }

        // GET /api/reports/me
        [RoleRequirement("Admin", "Master")]
        [HttpGet("me")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReportsForCurrentUserAsync()
        {
            var userId = User.GetUserID();
            var list = await Context.Reports
                                            .Where(r => r.GeneratedBy.Equals(userId))
                                            .ToListAsync();

            return Ok(list);
        }

        // GET /api/reports/{id}
        [RoleRequirement("Master")]
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportByIDAsync(
            Guid id
        )
        {
            var report = await Context.Reports
                                            .SingleOrDefaultAsync(r => r.ID.Equals(id));

            return Ok(report);
        }

        // GET /api/reports/me/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpGet("me/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportByIDForCurrentUserAsync(
            Guid id
        )
        {
            var userId = User.GetUserID();
            var report = await Context.Reports
                                            .SingleOrDefaultAsync(r => r.ID.Equals(id) && r.GeneratedBy.Equals(userId));

            return Ok(report);
        }
    }
}
