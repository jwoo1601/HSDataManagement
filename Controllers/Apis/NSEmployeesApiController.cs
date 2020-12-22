using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.Models;
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
    [RoleRequirement("Admin", "Master")]
    [Area("nutrition-support")]
    [Route("api/[area]/employees")]
    [ApiController]
    public class NSEmployeesApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Employee> Logger { get; }
        IStringLocalizer<NSEmployeesApiController> Localizer { get; }

        public NSEmployeesApiController(
            AppDbContext context,
            ILogger<Employee> logger,
            IStringLocalizer<NSEmployeesApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }
    }
}
