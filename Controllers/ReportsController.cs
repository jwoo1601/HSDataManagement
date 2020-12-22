using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using HyosungManagement.Controllers.Apis;
using HyosungManagement.Data;
using HyosungManagement.Extensions;
using HyosungManagement.Filters;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using HyosungManagement.Services;
using HyosungManagement.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace HyosungManagement.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Admin", "Master")]
    [Route("reports")]
    public class ReportsController : ApiControllerBase
    {
        public static readonly string QueryDateTimeFormat = "yyyyMMdd";

        AppDbContext Context { get; }
        ILogger<ReportsController> Logger { get; }
        IStringLocalizer<ReportsController> Localizer { get; }
        IViewRendererService ViewRenderer { get; }
        UserManager<HSMUser> UserManager { get; }

        public ReportsController(
            AppDbContext context,
            ILogger<ReportsController> logger,
            IStringLocalizer<ReportsController> localizer,
            IViewRendererService viewRenderer,
            UserManager<HSMUser> userManager
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
            ViewRenderer = viewRenderer;
            UserManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportByIDAsync(
            Guid? id
        )
        {
            if (id.HasValue)
            {
                var report = await Context.Reports.SingleOrDefaultAsync(r => r.ID.Equals(id.Value));
                if (report != null)
                {
                    var userId = User.GetUserID();
                    var userRole = User.GetFirstInRoles();
                    if (userRole != "Master" || !report.GeneratedBy.Equals(userId))
                    {
                        return Forbid();
                    }

                    try
                    {
                        var fileResult = await RenderOperationLogAsync(report);

                        Logger.LogInformation(
                            ReportsLogEvents.GetReportByID,
                            "Successfully retrieved report: {@Report}",
                            report
                        );

                        return fileResult;
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(
                            ReportsLogEvents.GetReportByIDError,
                            e,
                            "Failed to retrieve report: {@Report}",
                            report
                        );

                        return ServerError(Localizer["report.generateError"]);
                    }
                }
            }

            return NotFound();
        }

        [HttpGet("generate/operation-log")]
        public async Task<IActionResult> GenerateOperationLogAsync(
            [FromServices] ILogger<OperationLog> logLogger,
            [FromQuery] string date
        )
        {
            DateTime parsedDate;
            if (!DateTime.TryParseExact(
                    date,
                    QueryDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate
            ))
            {
                logLogger.LogError(
                    ReportsLogEvents.InvalidDate,
                    "Failed to generate operation log: invalid date {Date}.",
                    date
                );

                return BadRequest(Localizer["date.invalid"]);
            }

            var newReport = new Report
            {
                ID = Guid.NewGuid(),
                ReportType = ReportType.OperationLog,
                LogDate = parsedDate.Date,
                GeneratedBy = User.GetUserID()
            };
            Context.Reports.Add(newReport);
            await Context.SaveChangesAsync();

            try
            {
                var fileResult = await RenderOperationLogAsync(newReport);

                logLogger.LogInformation(
                    ReportsLogEvents.GenerateOperationLog,
                    "Successfully generated operation log: {@Report}",
                    newReport
                );

                return fileResult;
            }
            catch (Exception e)
            {
                logLogger.LogError(
                    ReportsLogEvents.GenerateOperationLogError,
                    e,
                    "Failed to generate operation log: {@Report}",
                    newReport
                );

                return ServerError(Localizer["operationLog.generateError"]);
            }
        }

        private async Task<FileStreamResult> RenderOperationLogAsync(Report report)
        {
            var fileStream = await ViewRenderer.RenderViewAsPdfAsync(
                "Reports/OperationLog",
                new ViewPdfRendererSettings
                {
                    MediaType = PdfRenderingMediaType.Print,
                    Orientation = PdfRenderingOrientation.Portrait,
                    SinglePageOption = PdfRenderingSinglePageOption.FixWidth
                },
                new OperationLogReportViewModel
                {
                    ReportID = report.ID,
                    LogDate = report.LogDate,
                    Menu = await Context.DailyMenus.SingleOrDefaultAsync(m => m.ServedDate.Date.Equals(report.LogDate)),
                    Customers = await Context.Customers.ToListAsync()
                }
            );

            return File(
                fileStream,
                MediaTypeNames.Application.Pdf,
                Localizer["fileName.operationLog", report.LogDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)]
            );
        }
    }
}
