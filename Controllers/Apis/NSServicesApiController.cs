using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Admin", "Master")]
    [Area("nutrition-support")]
    [Route("api/[area]/services")]
    [ApiController]
    public class NSServicesApiController : ApiControllerBase
    {
        public AppDbContext Context { get; }
        public ILogger<Service> Logger { get; }
        IStringLocalizer<NSServicesApiController> Localizer { get; }

        public NSServicesApiController(
            AppDbContext context,
            ILogger<Service> logger,
            IStringLocalizer<NSServicesApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }

        #region GetAll-
        // GET /api/nutrition-support/services
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllServicesAsync()
        {
            var list = await Context.Services.ToListAsync();

            Logger.LogInformation(
                ServicesApiLogEvents.GetAllServices,
                "Successfully retrieved list of services - num: {num}.",
                list.Count
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/services/groups
        [HttpGet("groups")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllServiceGroupsAsync(
            [FromServices] ILogger<ServiceGroup> groupLogger
        )
        {
            var list = await Context.ServiceGroups.ToListAsync();

            groupLogger.LogInformation(
                ServicesApiLogEvents.GetAllServiceGroups,
                "Successfully retrieved list of service groups - num: {num}.",
                list.Count
            );

            return Ok(list);
        }
        #endregion

        #region Get-ById
        // GET /api/nutrition-support/services/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceByIdAsync(int id)
        {
            var foundService = await Context.Services.SingleOrDefaultAsync(s => s.ID == id);
            if (foundService == null)
            {
                Logger.LogError(
                    ServicesApiLogEvents.ServiceNotFound,
                    "Failed to retrieve service with id {ID}: service not found.",
                    id
                );

                return NotFound(Localizer["service.notFound"]);
            }

            Logger.LogInformation(
                ServicesApiLogEvents.GetServiceByID,
                "Successfully retrieved service {@Service}.",
                foundService
            );

            return Ok(foundService);
        }

        // GET /api/nutrition-support/services/groups/{id}
        [HttpGet("groups/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceGroupByIdAsync(
            [FromServices] ILogger<ServiceGroup> groupLogger,
            int id
        )
        {
            var foundGroup = await Context.ServiceGroups
                                            
                                            .SingleOrDefaultAsync(sg => sg.ID == id);
            if (foundGroup == null)
            {
                groupLogger.LogError(
                    ServicesApiLogEvents.ServiceGroupNotFound,
                    "Failed to retrieve service group with id {ID}: group not found.",
                    id
                );

                return NotFound(Localizer["serviceGroup.notFound"]);
            }

            groupLogger.LogInformation(
                ServicesApiLogEvents.GetServiceGroupByID,
                "Successfully retrieved service group {@Group}.",
                foundGroup
            );

            return Ok(foundGroup);
        }
        #endregion

        #region Add
        // POST /api/nutrition-support/services
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddServiceAsync(
            [FromBody] ServiceInputModel inputModel
        )
        {
            try
            {
                var foundGroup = await Context.ServiceGroups
                                                
                                                .SingleOrDefaultAsync(sg => sg.ID == inputModel.Group);
                if (foundGroup == null)
                {
                    Logger.LogError(
                        ServicesApiLogEvents.ServiceGroupNotFound,
                        "Failed to add service with {@InputModel}: no matching group found",
                        inputModel
                    );

                    return NotFound(Localizer["serviceGroup.notFound"]);
                }

                var service = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                Logger.LogInformation(
                    ServicesApiLogEvents.AddService,
                    "Successfully added new service {@Service} with {@InputModel}.",
                    service,
                    inputModel
                );

                return Created(service);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    ServicesApiLogEvents.AddServiceError,
                    e,
                    "Failed to add new service with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["service.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/services/groups
        [HttpPost("groups")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddServiceGroupAsync(
            [FromServices] ILogger<ServiceGroup> groupLogger,
            [FromBody] ServiceGroupInputModel inputModel
        )
        {
            try
            {
                var group = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                groupLogger.LogInformation(
                    ServicesApiLogEvents.AddServiceGroup,
                    "Successfully added new service group {@Group} with {@InputModel}.",
                    group,
                    inputModel
                );

                return Created(group);
            }
            catch (DbUpdateException e)
            {
                groupLogger.LogError(
                    ServicesApiLogEvents.AddServiceGroupError,
                    e,
                    "Failed to add service group with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["serviceGroup.dbFailure"]);
            }
        }
        #endregion

        #region Edit
        // PUT /api/nutrition-support/services/{id}
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditServiceAsync(
            int id,
            [FromBody] ServiceInputModel inputModel
        )
        {
            try
            {
                var foundService = await Context.Services.SingleOrDefaultAsync(
                    s => s.ID == id
                );
                if (foundService == null)
                {
                    Logger.LogError(
                        ServicesApiLogEvents.ServiceNotFound,
                        "Failed to update service {ID} with {@InputModel}: no matching service found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["service.notFound"]);
                }

                var foundGroup = await Context.ServiceGroups
                                                
                                                .SingleOrDefaultAsync(sg => sg.ID == inputModel.Group);
                if (foundGroup == null)
                {
                    Logger.LogError(
                        ServicesApiLogEvents.ServiceGroupNotFound,
                        "Failed to update service {ID} with {@InputModel}: no matching group found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["serviceGroup.notFound"]);
                }

                var service = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                Logger.LogInformation(
                    ServicesApiLogEvents.EditService,
                    "Successfully updated service {@Service} with {@InputModel}.",
                    service,
                    inputModel
                );

                return Ok(service);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    ServicesApiLogEvents.EditServiceError,
                    e,
                    "Failed to update service {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["service.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/services/groups/{id}
        [HttpPut("groups/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditServiceGroupAsync(
            [FromServices] ILogger<ServiceGroup> groupLogger,
            int id,
            [FromBody] ServiceGroupInputModel inputModel
        )
        {
            try
            {
                var foundGroup = await Context.ServiceGroups.SingleOrDefaultAsync(
                    sg => sg.ID == id
                );
                if (foundGroup == null)
                {
                    groupLogger.LogError(
                        ServicesApiLogEvents.ServiceGroupNotFound,
                        "Failed to update service group {ID} with {@InputModel}: no matching group found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["serviceGroup.notFound"]);
                }

                var group = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                groupLogger.LogInformation(
                    ServicesApiLogEvents.EditServiceGroup,
                    "Successfully updated service group {@Group} with {@InputModel}.",
                    group,
                    inputModel
                );

                return Ok(group);
            }
            catch (DbUpdateException e)
            {
                groupLogger.LogError(
                    ServicesApiLogEvents.EditServiceGroupError,
                    e,
                    "Failed to update service group {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["serviceGroup.dbFailure"]);
            }
        }
        #endregion

        #region Delete
        // DELETE /api/nutrition-support/services/{id}
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteServiceAsync(int id)
        {
            try
            {
                var foundService = await Context.Services.SingleOrDefaultAsync(s => s.ID == id);
                if (foundService == null)
                {
                    Logger.LogError(
                        ServicesApiLogEvents.ServiceNotFound,
                        "Failed to delete service {ID}: no matching service found",
                        id
                    );

                    return NotFound(Localizer["service.notFound"]);
                }

                Context.Remove(foundService);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    ServicesApiLogEvents.DeleteService,
                    "Successfully deleted service {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    ServicesApiLogEvents.DeleteServiceError,
                    e,
                    "Failed to delete service {ID}.",
                    id
                );

                return ServerError(Localizer["service.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/services/groups/{id}
        [HttpDelete("groups/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteServiceGroupAsync(
            [FromServices] ILogger<ServiceGroup> groupLogger,
            int id
        )
        {
            try
            {
                var foundGroup = await Context.ServiceGroups.SingleOrDefaultAsync(sg => sg.ID == id);
                if (foundGroup == null)
                {
                    groupLogger.LogError(
                        ServicesApiLogEvents.ServiceGroupNotFound,
                        "Failed to delete service group {ID}: no matching group found",
                        id
                    );

                    return NotFound(Localizer["serviceGroup.notFound"]);
                }

                Context.Remove(foundGroup);
                await Context.SaveChangesAsync();

                groupLogger.LogInformation(
                    ServicesApiLogEvents.DeleteServiceGroup,
                    "Successfully deleted service group {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                groupLogger.LogError(
                    ServicesApiLogEvents.DeleteServiceGroupError,
                    e,
                    "Failed to delete service group {ID}.",
                    id
                );

                return ServerError(Localizer["serviceGroup.deleteFailure"]);
            }
        }
        #endregion
    }
}
