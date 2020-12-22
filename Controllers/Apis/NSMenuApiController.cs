using HyosungManagement.Data;
using HyosungManagement.Extensions;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("User", "Admin", "Master")]
    [Area("nutrition-support")]
    [Route("api/[area]/menu")]
    [ApiController]
    public class NSMenuApiController : ApiControllerBase
    {
        public static readonly string QueryDateTimeFormat = "yyyyMMdd";

        AppDbContext Context { get; }
        ILogger<DailyMenu> Logger { get; }
        IStringLocalizer<NSMenuApiController> Localizer { get; }

        public NSMenuApiController(
            AppDbContext context,
            ILogger<DailyMenu> logger,
            IStringLocalizer<NSMenuApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }

        #region GetAll-
        // GET /api/nutrition-support/menu?from={Date}[&to={Date}]
        // Date: yyyyMMdd
        // [from, to)
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMenusInPeriodAsync(
            [FromQuery] string from,
            [FromQuery] string to
        )
        {
            // [startDate, endDate)
            var startDate = from.ParseUrlEncodedDate();
            if (startDate == null)
            {
                Logger.LogError(
                    MenuApiLogEvents.InvalidStartDate,
                    "Failed to retrieve menu list: invalid start Date {Date}.",
                    from
                );

                return BadRequest(Localizer["startDate.invalid"]);
            }

            DateTime? endDate;
            if (string.IsNullOrWhiteSpace(to))
            {
                endDate = DateTime.UtcNow;
            }
            else
            {
                endDate = to.ParseUrlEncodedDate();
                if (endDate == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.InvalidEndDate,
                        "Failed to retrieve menu list: invalid end Date {Date}.",
                        to
                    );

                    return BadRequest(Localizer["endDate.invalid"]);
                }
            }

            var list = (await Context.DailyMenus.ToListAsync()).Where(
                m => m.ServedDate.Between(startDate.Value, endDate.Value)
            );

            Logger.LogInformation(
                MenuApiLogEvents.GetMenusInPeriod,
                "Successfully retrieved menus in time period [{From}, {To}) - num: {num}.",
                startDate?.ToSimpleDateString(),
                endDate?.ToSimpleDateString(),
                list.Count()
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/menu/logs?type={MenuLogType}&from={Date}[&to={Date}]
        [RoleRequirement("Admin", "Master")]
        [HttpGet("logs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLogsInPeriodAsync(
            [FromQuery] string type,
            [FromQuery] string from,
            [FromQuery] string to
        )
        {
            var logType = type.ParseUrlEncodedEnum<MenuLogType>();
            if (logType == null)
            {
                Logger.LogError(
                    MenuApiLogEvents.InvalidLogType,
                    "Failed to retrieve list of logs: invalid log type {Type}.",
                    logType?.GetName()
                );

                return BadRequest(Localizer["logType.invalid"]);
            }

            var services = HttpContext.RequestServices;

            ILogger logLogger = null;
            Func<IEnumerable<DailyMenu>, IEnumerable<object>> logTransformer = null;
            switch (logType)
            {
                case MenuLogType.Operation:
                    logLogger = services.GetRequiredService<ILogger<OperationLog>>();
                    logTransformer = (list) => list.SelectMany(m => m.OperationLogs);
                    break;
                case MenuLogType.Preservation:
                    logLogger = services.GetRequiredService<ILogger<PreservationLog>>();
                    logTransformer = (list) => list.SelectMany(m => m.PreservationLogs);
                    break;
                case MenuLogType.Inspection:
                    //logLogger = services.GetRequiredService<ILogger<InspectionLog>>()
                    //logTransformer = (list) => list.Select(m => m.InspectionLog);
                    logLogger = Logger;
                    logTransformer = (list) => list.Select(m => new { });
                    break;
            }

            // [startDate, endDate)
            var startDate = from.ParseUrlEncodedDate();
            if (startDate == null)
            {
                logLogger.LogError(
                    MenuApiLogEvents.InvalidStartDate,
                    "Failed to retrieve list of {Type} logs: invalid start Date {Date}.",
                    type,
                    from
                );

                return BadRequest(Localizer["startDate.invalid"]);
            }

            DateTime? endDate;
            if (string.IsNullOrWhiteSpace(to))
            {
                endDate = DateTime.UtcNow;
            }
            else
            {
                endDate = to.ParseUrlEncodedDate();
                if (endDate == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidEndDate,
                        "Failed to retrieve list of {Type} logs: invalid end Date {Date}.",
                        type,
                        to
                    );

                    return BadRequest(Localizer["endDate.invalid"]);
                }
            }

            var menuList = (await Context.DailyMenus.ToListAsync()).Where(
                m => m.ServedDate.Between(startDate.Value, endDate.Value)
            );
            var logs = logTransformer(menuList);

            logLogger.LogInformation(
                MenuApiLogEvents.GetLogsInPeriod,
                "Successfully retrieved list of {Type} logs [{From}, {To}) - num: {num}.",
                type,
                startDate?.ToSimpleDateString(),
                endDate?.ToSimpleDateString(),
                logs.Count()
            );

            return Ok(logs);
        }
        #endregion

        #region Get-ById
        // GET /api/nutrition-support/menu/detail?date={Date}
        [HttpGet("detail")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMenuByDateAsync(
            [FromQuery] string date
        )
        {
            DateTime? parsedDate;
            if (string.IsNullOrWhiteSpace(date))
            {
                parsedDate = DateTime.UtcNow;
            }
            else
            {
                parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to retrieve menu detail: invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }
            }

            var menu = await Context.DailyMenus.SingleOrDefaultAsync(
                m => m.ServedDate.Equals(parsedDate.Value)
            );

            Logger.LogInformation(
                MenuApiLogEvents.GetMenuByDate,
                "Successfully retrieved menu detail for {Date}.",
                parsedDate?.ToSimpleDateString()
            );

            return Ok(
                new
                {
                    menu.ID,
                    menu.ServedDate,
                    menu.Note,
                    menu.CreatedAt,
                    menu.LastUpdatedAt,
                    Operations = menu.OperationLogs,
                    Preservations = menu.PreservationLogs
                }
            );
        }

        // GET /api/nutrition-support/menu/logs/operation?date={Date}[&meal_type={MealType}]
        [RoleRequirement("Admin", "Master")]
        [HttpGet("logs/operation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOperationLogsAsync(
            [FromServices] ILogger<OperationLog> logLogger,
            [FromQuery] string date,
            [FromQuery(Name = "meal_type")] string type
        )
        {
            DateTime? parsedDate;
            if (string.IsNullOrWhiteSpace(date))
            {
                parsedDate = DateTime.UtcNow;
            }
            else
            {
                parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to retrieve operation logs: invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }
            }

            MealType? mealType = null;
            bool bGetAllMealTypes = false;
            if (string.IsNullOrWhiteSpace(type))
            {
                bGetAllMealTypes = true;
            }
            else
            {
                mealType = type.ParseUrlEncodedEnum<MealType>();
                if (mealType == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidMealType,
                        "Failed to retrieve operation logs: invalid meal type {Type}.",
                        type
                    );

                    return BadRequest(Localizer["mealType.invalid"]);
                }
            }

            var menu = await Context.DailyMenus.SingleOrDefaultAsync(
                m => m.ServedDate.Equals(parsedDate)
            );

            IEnumerable<OperationLog> result;
            if (bGetAllMealTypes)
            {
                result = menu.OperationLogs;

                logLogger.LogInformation(
                    MenuApiLogEvents.GetOperationLogs,
                    "Successfully retrieved all operation logs for {Date} - num: {Num}.",
                    parsedDate?.ToSimpleDateString(),
                    result.Count()
                );
            }
            else
            {
                result = new[] { menu.OperationLogs.SingleOrDefault(op => op.MealType == mealType) };

                logLogger.LogInformation(
                    MenuApiLogEvents.GetOperationLogs,
                    "Successfully retrieved operation log for {Type} at {Date}.",
                    mealType?.GetName(),
                    parsedDate?.ToSimpleDateString()
                );
            }

            return Ok(result);
        }

        // GET /api/nutrition-support/menu/logs/preservation?date={Date}[&meal_type={MealType}][&meal_category={MealCategory}]
        [RoleRequirement("Admin", "Master")]
        [HttpGet("logs/preservation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPreservationLogsAsync(
            [FromServices] ILogger<PreservationLog> logLogger,
            [FromQuery] string date,
            [FromQuery(Name = "meal_type")] string type,
            [FromQuery(Name = "meal_category")] string category
        )
        {
            DateTime? parsedDate;
            if (string.IsNullOrWhiteSpace(date))
            {
                parsedDate = DateTime.Now;
            }
            else
            {
                parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to retrieve preservation logs: invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }
            }

            MealType? mealType = null;
            bool bGetAllMealTypes = false;
            if (string.IsNullOrWhiteSpace(type))
            {
                bGetAllMealTypes = true;
            }
            else
            {
                mealType = type.ParseUrlEncodedEnum<MealType>();
                if (mealType == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidMealType,
                        "Failed to retrieve preservation logs: invalid meal type {Type}.",
                        type
                    );

                    return BadRequest(Localizer["mealType.invalid"]);
                }
            }

            MealCategory? mealCategory = null;
            bool bGetAllMealCategories = false;
            if (string.IsNullOrWhiteSpace(category))
            {
                bGetAllMealCategories = true;
            }
            else
            {
                mealCategory = category.ParseUrlEncodedEnum<MealCategory>();
                if (mealCategory == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.InvalidMealCategory,
                        "Failed to retrieve preservation logs: invalid meal category {Category}.",
                        category
                    );

                    return BadRequest(Localizer["mealCategory.invalid"]);
                }
            }

            var menu = await Context.DailyMenus.SingleOrDefaultAsync(
                m => m.ServedDate.Equals(parsedDate)
            );

            IEnumerable<PreservationLog> result;
            if (bGetAllMealTypes)
            {
                result = menu.PreservationLogs;
            }
            else
            {
                result = new[] { menu.PreservationLogs.SingleOrDefault(op => op.MealType == mealType) };
            }

            if (bGetAllMealCategories)
            {
                logLogger.LogInformation(
                     MenuApiLogEvents.GetPreservationLogs,
                     "Successfully retrieved all preservation logs for {Date} - num: {Num}.",
                     parsedDate?.ToSimpleDateString(),
                     result.Count()
                 );
            }
            else
            {
                result = result.Where(pr => pr.MealCategory == mealCategory);

                logLogger.LogInformation(
                    MenuApiLogEvents.GetOperationLogs,
                    "Successfully retrieved operation log for {Type} at {Date}.",
                    mealType?.GetName(),
                    parsedDate?.ToSimpleDateString()
                );

            }

            return Ok(result);
        }
        #endregion

        #region Add
        // POST /api/nutrition-support/menu
        [RoleRequirement("Admin", "Master")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMenuAsync(
            [FromBody] DailyMenuInputModel inputModel
        )
        {
            try
            {
                var existingMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    dm => dm.ServedDate.Equals(inputModel.ServedDate.Date)
                );
                if (existingMenu != null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.DuplicateMenu,
                        "Failed to add menu with {@InputModel}: menu already exists for the date.",
                        inputModel
                    );

                    return BadRequest(Localizer["menu.duplicateError"]);
                }

                var foundPackage = await Context.MealPackages
                                                
                                                .SingleOrDefaultAsync(mp => mp.ID == inputModel.Package);
                if (foundPackage == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MealPackageNotFound,
                        "Failed to add menu with {@InputModel}: meal package not found.",
                        inputModel
                    );

                    return NotFound(Localizer["mealPackage.notFound"]);
                }

                var menu = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                Logger.LogInformation(
                    MenuApiLogEvents.AddMenu,
                    "Successfully added new menu {@Menu} with {@InputModel}.",
                    menu,
                    inputModel
                );

                return Created(menu);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MenuApiLogEvents.AddMenuError,
                    e,
                    "Failed to add new menu with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["menu.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/menu/logs/operation
        [RoleRequirement("Admin", "Master")]
        [HttpPost("logs/operation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOperationLogAsync(
            [FromServices] ILogger<OperationLog> logLogger,
            [FromBody] OperationLogInputModel inputModel
        )
        {
            try
            {
                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    dm => dm.ServedDate.Equals(inputModel.LogDate.Date)
                );
                if (foundMenu == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to add operation log entry with {@InputModel}: menu not found.",
                        inputModel
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                if (
                    foundMenu.OperationLogs.Where(op => op.MealType == inputModel.MealType).Any()
                )
                {
                    logLogger.LogError(
                        MenuApiLogEvents.DuplicateOperationLog,
                        "Failed to add operation log entry with {@InputModel}: duplicate meal type {MealType}.",
                        inputModel,
                        inputModel.MealType.ToString("G")
                    );

                    return BadRequest(Localizer["operationLog.duplicateError"]);
                }

                var log = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                logLogger.LogInformation(
                    MenuApiLogEvents.AddOperationLog,
                    "Successfully added new operation log entry {@Log} with {@InputModel}.",
                    log,
                    inputModel
                );

                return Created(log);
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.AddOperationLogError,
                    e,
                    "Failed to add operation log entry with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["operationLog.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/menu/logs/preservation
        [RoleRequirement("Admin", "Master")]
        [HttpPost("logs/preservation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPreservationLogAsync(
            [FromServices] ILogger<PreservationLog> logLogger,
            [FromBody] PreservationLogInputModel inputModel
        )
        {
            try
            {
                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    dm => dm.ServedDate.Equals(inputModel.LogDate.Date)
                );
                if (foundMenu == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to add preservation log entry with {@InputModel}: menu not found.",
                        inputModel
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                if (
                    foundMenu.PreservationLogs.Where(
                        op => op.MealType == inputModel.MealType &&
                        op.MealCategory == inputModel.MealCategory
                    ).Any()
                )
                {
                    logLogger.LogError(
                        MenuApiLogEvents.DuplicatePreservationLog,
                        "Failed to add preservation log entry with {@InputModel}: duplicate meal type {MealType} and meal category {Category}.",
                        inputModel,
                        inputModel.MealType.ToString("G"),
                        inputModel.MealCategory.ToString("G")
                    );

                    return BadRequest(Localizer["preservationLog.duplicateError"]);
                }

                var foundRole = await Context.EmployeeRoles.SingleOrDefaultAsync(
                    er => er.ID == inputModel.Manager
                );
                if (foundRole == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.EmployeeRoleNotFound,
                        "Failed to add preservation log entry with {@InputModel}: employee role not found.",
                        inputModel
                    );

                    return NotFound(Localizer["employeeRole.notFound"]);
                }

                var log = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                logLogger.LogInformation(
                    MenuApiLogEvents.AddPreservationLog,
                    "Successfully added new preservation log entry {@Log} with {@InputModel}.",
                    log,
                    inputModel
                );

                return Created(log);
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.AddPreservationLog,
                    e,
                    "Failed to add preservation log entry with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["preservationLog.dbFailure"]);
            }
        }
        #endregion

        #region Edit
        // PUT /api/nutrition-support/menu
        [RoleRequirement("Admin", "Master")]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditMenuAsync(
            [FromBody] DailyMenuInputModel inputModel
        )
        {
            var date = inputModel.ServedDate.Date;
            try
            {
                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    m => m.ServedDate.Equals(date)
                );
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to update menu for {Date} with {@InputModel}: no matching menu found",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                var foundPackage = await Context.MealPackages.SingleOrDefaultAsync(
                    mp => mp.ID == inputModel.Package
                );
                if (foundPackage == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MealPackageNotFound,
                        "Failed to edit menu with {@InputModel}: meal package not found.",
                        inputModel
                    );

                    return NotFound(Localizer["mealPackage.notFound"]);
                }

                var menu = await inputModel.SaveAsEntityAsync(
                    foundMenu.ID,
                    Context,
                    HttpContext.RequestServices
                );

                Logger.LogInformation(
                    MenuApiLogEvents.EditMenu,
                    "Successfully updated menu {@Menu} with {@InputModel}.",
                    menu,
                    inputModel
                );

                return Ok(menu);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MenuApiLogEvents.EditMenuError,
                    e,
                    "Failed to update menu for {Date} with {@InputModel}.",
                    date.ToSimpleDateString(),
                    inputModel
                );

                return ServerError(Localizer["menu.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/menu/logs/operation
        [RoleRequirement("Admin", "Master")]
        [HttpPut("logs/operation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditOperationLogAsync(
            [FromServices] ILogger<OperationLog> logLogger,
            [FromBody] OperationLogInputModel inputModel
        )
        {
            var date = inputModel.LogDate.Date;
            try
            {
                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    m => m.ServedDate.Equals(date)
                );
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to update operation log for {Date} with {@InputModel}: no matching menu found",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                var foundLog = foundMenu.OperationLogs.SingleOrDefault(op => op.MealType == inputModel.MealType);
                if (foundLog == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.OperationLogNotFound,
                        "Failed to update operation log for {Date} with {@InputModel}: no matching log found",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["operationLog.notFound"]);
                }

                var log = await inputModel.SaveAsEntityAsync(
                    foundLog.ID,
                    Context,
                    HttpContext.RequestServices
                );

                logLogger.LogInformation(
                    MenuApiLogEvents.EditOperationLog,
                    "Successfully updated operation log {@Log} with {@InputModel}.",
                    log,
                    inputModel
                );

                return Ok(log);
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.EditOperationLogError,
                    e,
                    "Failed to update operation log for {Date} with {@InputModel}.",
                    date.ToSimpleDateString(),
                    inputModel
                );

                return ServerError(Localizer["operationLog.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/menu/logs/preservation
        [RoleRequirement("Admin", "Master")]
        [HttpPut("logs/preservation")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditPreservationLogAsync(
            [FromServices] ILogger<PreservationLog> logLogger,
            [FromBody] PreservationLogInputModel inputModel
        )
        {
            var date = inputModel.LogDate.Date;
            try
            {
                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(
                    m => m.ServedDate.Equals(date)
                );
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to update preservation log for {Date} with {@InputModel}: no matching menu found",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                var foundRole = await Context.EmployeeRoles.SingleOrDefaultAsync(
                    er => er.ID == inputModel.Manager
                );
                if (foundRole == null)
                {
                    logLogger.LogError(
                        MenuApiLogEvents.EmployeeRoleNotFound,
                        "Failed to update preservation log for {Date} with {@InputModel}: employee role not found.",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["employeeRole.notFound"]);
                }

                var foundLog = foundMenu.PreservationLogs.SingleOrDefault(
                    pr =>
                        pr.MealType == inputModel.MealType &&
                        pr.MealCategory == inputModel.MealCategory
                );
                if (foundLog == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.PreservationLogNotFound,
                        "Failed to update preservation log for {Date} with {@InputModel}: no matching log found",
                        date.ToSimpleDateString(),
                        inputModel
                    );

                    return NotFound(Localizer["preservationLog.notFound"]);
                }

                var log = await inputModel.SaveAsEntityAsync(
                    foundLog.ID,
                    Context,
                    HttpContext.RequestServices
                );

                logLogger.LogInformation(
                    MenuApiLogEvents.EditPreservationLog,
                    "Successfully updated preservation log {@Log} with {@InputModel}.",
                    log,
                    inputModel
                );

                return Ok(log);
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.EditPreservationLogError,
                    e,
                    "Failed to update preservation log for {Date} with {@InputModel}.",
                    date.ToSimpleDateString(),
                    inputModel
                );

                return ServerError(Localizer["preservationLog.dbFailure"]);
            }
        }
        #endregion

        #region Delete
        // DELETE /api/nutrition-support/menu/{date}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("{date}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMenuAsync(string date)
        {
            try
            {
                var parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to delete menu: invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }

                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(dm => dm.ServedDate.Equals(parsedDate));
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to delete menu for {Date}: no matching menu found",
                        parsedDate?.ToSimpleDateString()
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                Context.Remove(foundMenu);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    MenuApiLogEvents.DeleteMenu,
                    "Successfully deleted menu for {Date}.",
                    parsedDate?.ToSimpleDateString()
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MenuApiLogEvents.DeleteMenuError,
                    e,
                    "Failed to delete menu for {Date}.",
                    date
                );

                return ServerError(Localizer["menu.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/menu/logs/operation/{date}?meal_type={MealType}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("logs/operation/{date}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOperationLogAsync(
            [FromServices] ILogger<OperationLog> logLogger,
            string date,
            [FromQuery(Name = "meal_type")] string type
        )
        {
            try
            {
                var parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to delete operation log(s): invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }

                MealType? mealType = null;
                bool bDeleteAllMealTypes = false;
                if (string.IsNullOrWhiteSpace(type))
                {
                    bDeleteAllMealTypes = true;
                }
                else
                {
                    mealType = type.ParseUrlEncodedEnum<MealType>();
                    if (mealType == null)
                    {
                        logLogger.LogError(
                            MenuApiLogEvents.InvalidMealType,
                            "Failed to delete operation log(s): invalid meal type {Type}.",
                            type
                        );

                        return BadRequest(Localizer["mealType.invalid"]);
                    }
                }

                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(dm => dm.ServedDate.Equals(parsedDate));
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to delete operation log(s) for {Date} and {Type}: no matching menu found",
                        parsedDate?.ToSimpleDateString(),
                        mealType?.GetName()
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                IEnumerable<OperationLog> foundLogs;
                if (bDeleteAllMealTypes)
                {
                    foundLogs = foundMenu.OperationLogs;
                }
                else
                {
                    foundLogs = new[] { foundMenu.OperationLogs.SingleOrDefault(op => op.MealType == mealType) };
                }
                if (!foundLogs.Any())
                {
                    Logger.LogError(
                        MenuApiLogEvents.OperationLogNotFound,
                        "Failed to delete operation log(s) for {Date} and {Type}: no matching log(s) found",
                        parsedDate?.ToSimpleDateString(),
                        mealType?.GetName()
                    );

                    return NotFound(Localizer["operationLog.notFound"]);
                }

                Context.RemoveRange(foundLogs);
                await Context.SaveChangesAsync();

                logLogger.LogInformation(
                    MenuApiLogEvents.DeleteOperationLog,
                    "Successfully deleted operation log(s) for {Date} and {Type}.",
                    parsedDate?.ToSimpleDateString(),
                    mealType?.GetName()
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.DeleteOperationLogError,
                    e,
                    "Failed to delete operation log(s) for {Date} and {Type}.",
                    date,
                    type
                );

                return ServerError(Localizer["operationLog.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/menu/logs/preservation/{date}?meal_type={MealType}&meal_category={MealCategory}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("logs/preservation/{date}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePreservationLogAsync(
            [FromServices] ILogger<PreservationLog> logLogger,
            string date,
            [FromQuery(Name = "meal_type")] string type,
            [FromQuery(Name = "meal_category")] string category
        )
        {
            try
            {
                var parsedDate = date.ParseUrlEncodedDate();
                if (parsedDate == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.InvalidDate,
                        "Failed to delete preservation log(s): invalid date {Date}.",
                        date
                    );

                    return BadRequest(Localizer["date.invalid"]);
                }

                MealType? mealType = null;
                bool bDeleteAllMealTypes = false;
                if (string.IsNullOrWhiteSpace(type))
                {
                    bDeleteAllMealTypes = true;
                }
                else
                {
                    mealType = type.ParseUrlEncodedEnum<MealType>();
                    if (mealType == null)
                    {
                        logLogger.LogError(
                            MenuApiLogEvents.InvalidMealType,
                            "Failed to delete preservation log(s) for {Date}: invalid meal type {Type}.",
                            parsedDate?.ToSimpleDateString(),
                            type
                        );

                        return BadRequest(Localizer["mealType.invalid"]);
                    }
                }

                MealCategory? mealCategory = null;
                bool bDeleteAllMealCategories = false;
                if (string.IsNullOrWhiteSpace(category))
                {
                    bDeleteAllMealCategories = true;
                }
                else
                {
                    mealCategory = type.ParseUrlEncodedEnum<MealCategory>();
                    if (mealCategory == null)
                    {
                        logLogger.LogError(
                            MenuApiLogEvents.InvalidMealCategory,
                            "Failed to delete preservation log(s) for {Date} and {Type}: invalid meal category {Category}.",
                            parsedDate?.ToSimpleDateString(),
                            mealType?.GetName(),
                            category
                        );

                        return BadRequest(Localizer["mealCategory.invalid"]);
                    }
                }

                var foundMenu = await Context.DailyMenus.SingleOrDefaultAsync(dm => dm.ServedDate.Equals(parsedDate));
                if (foundMenu == null)
                {
                    Logger.LogError(
                        MenuApiLogEvents.MenuNotFound,
                        "Failed to delete preservation log(s) for {Date}, {Type} and {Category}: no matching menu found",
                        parsedDate?.ToSimpleDateString(),
                        mealType?.GetName(),
                        category
                    );

                    return NotFound(Localizer["menu.notFound"]);
                }

                IEnumerable<PreservationLog> foundLogs;
                if (bDeleteAllMealTypes)
                {
                    foundLogs = foundMenu.PreservationLogs;
                }
                else
                {
                    foundLogs = new[] { foundMenu.PreservationLogs.SingleOrDefault(op => op.MealType == mealType) };
                }
                if (!bDeleteAllMealCategories)
                {
                    foundLogs = foundLogs.Where(pr => pr.MealCategory == mealCategory);
                }

                if (!foundLogs.Any())
                {
                    Logger.LogError(
                        MenuApiLogEvents.PreservationLogNotFound,
                        "Failed to delete preservation log(s) for {Date}, {Type} and {Category}: no matching log(s) found",
                        parsedDate?.ToSimpleDateString(),
                        mealType?.GetName(),
                        mealCategory?.GetName()
                    );

                    return NotFound(Localizer["preservationLog.notFound"]);
                }

                Context.RemoveRange(foundLogs);
                await Context.SaveChangesAsync();

                logLogger.LogInformation(
                    MenuApiLogEvents.DeletePreservationLog,
                    "Successfully deleted preservation log(s) for {Date}, {Type} and {Category}.",
                    parsedDate?.ToSimpleDateString(),
                    mealType?.GetName(),
                    mealCategory?.GetName()
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                logLogger.LogError(
                    MenuApiLogEvents.DeletePreservationLogError,
                    e,
                    "Failed to delete operation log(s) for {Date}, {Type} and {Category}.",
                    date,
                    type,
                    category
                );

                return ServerError(Localizer["preservationLog.deleteFailure"]);
            }
        }
        #endregion
    }
}
