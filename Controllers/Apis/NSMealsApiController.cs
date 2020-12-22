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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Area("nutrition-support")]
    [Route("api/[area]/meals")]
    [ApiController]
    public class NSMealsApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Meal> Logger { get; }
        IStringLocalizer<NSMealsApiController> Localizer { get; }

        public NSMealsApiController(
            AppDbContext context,
            ILogger<Meal> logger,
            IStringLocalizer<NSMealsApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }

        #region GetAll-
        // GET /api/nutrition-support/meals
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMealsAsync()
        {
            var list = await Context.Meals.ToListAsync();

            Logger.LogInformation(
                MealsApiLogEvents.GetAllMeals,
                "Successfully retrieved list of meals - num: {num}.",
                list.Count
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/meals/packages
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("packages")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMealPackagesAsync(
            [FromServices] ILogger<MealPackage> packageLogger
        )
        {
            var list = await Context.MealPackages.ToListAsync();

            packageLogger.LogInformation(
                MealsApiLogEvents.GetAllMealPackages,
                "Successfully retrieved list of meal packages - num: {num}.",
                list.Count
            );

            return Ok(list);
        }
        #endregion

        #region Get-ById
        // GET /api/nutrition-support/meals/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMealByIdAsync(int id)
        {
            var foundMeal = await Context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            if (foundMeal == null)
            {
                Logger.LogError(
                    MealsApiLogEvents.MealNotFound,
                    "Failed to retrieve meal with id {ID}: meal not found.",
                    id
                );

                return NotFound(Localizer["meal.notFound"]);
            }

            Logger.LogInformation(
                MealsApiLogEvents.GetMealByID,
                "Successfully retrieved meal {@Meal}.",
                foundMeal
            );

            return Ok(foundMeal);
        }

        // GET /api/nutrition-support/meals/packages/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("packages/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMealPackageByIdAsync(
            [FromServices] ILogger<MealPackage> packageLogger,
            int id
        )
        {
            var foundPackage = await Context.MealPackages
                                            
                                            .SingleOrDefaultAsync(mp => mp.ID == id);
            if (foundPackage == null)
            {
                packageLogger.LogError(
                    MealsApiLogEvents.MealPackageNotFound,
                    "Failed to retrieve meal package with id {ID}: package not found.",
                    id
                );

                return NotFound(Localizer["mealPackage.notFound"]);
            }

            packageLogger.LogInformation(
                MealsApiLogEvents.GetMealPackageByID,
                "Successfully retrieved meal package {@Package}.",
                foundPackage
            );

            return Ok(foundPackage);
        }
        #endregion

        #region Add
        // POST /api/nutrition-support/meals
        [RoleRequirement("Admin", "Master")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMealAsync(
            [FromBody] MealInputModel inputModel
        )
        {
            try
            {
                var meal = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                Logger.LogInformation(
                    MealsApiLogEvents.AddMeal,
                    "Successfully added new meal {@Meal} with {@InputModel}.",
                    meal,
                    inputModel
                );

                return Created(meal);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MealsApiLogEvents.AddMealError,
                    e,
                    "Failed to add new meal with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["meal.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/meals/packages
        [RoleRequirement("Admin", "Master")]
        [HttpPost("packages")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMealPackageAsync(
            [FromServices] ILogger<MealPackage> packageLogger,
            [FromBody] MealPackageInputModel inputModel
        )
        {
            try
            {
                var package = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                packageLogger.LogInformation(
                    MealsApiLogEvents.AddMealPackage,
                    "Successfully added new meal package {@Package} with {@InputModel}.",
                    package,
                    inputModel
                );

                return Created(package);
            }
            catch (DbUpdateException e)
            {
                packageLogger.LogError(
                    MealsApiLogEvents.AddMealPackageError,
                    e,
                    "Failed to add meal package with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["mealPackage.dbFailure"]);
            }
        }
        #endregion

        #region Edit
        // PUT /api/nutrition-support/meals/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditMealAsync(
            int id,
            [FromBody] MealInputModel inputModel
        )
        {
            try
            {
                var foundMeal = await Context.Meals.SingleOrDefaultAsync(m => m.ID == id);
                if (foundMeal == null)
                {
                    Logger.LogError(
                        MealsApiLogEvents.MealNotFound,
                        "Failed to update meal {ID} with {@InputModel}: no matching meal found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["meal.notFound"]);
                }

                var meal = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                Logger.LogInformation(
                    MealsApiLogEvents.EditMeal,
                    "Successfully updated meal {@Meal} with {@InputModel}.",
                    meal,
                    inputModel
                );

                return Ok(meal);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MealsApiLogEvents.EditMealError,
                    e,
                    "Failed to update meal {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["meal.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/meals/packages/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("packages/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditMealPackageAsync(
            [FromServices] ILogger<MealPackage> packageLogger,
            int id,
            [FromBody] MealPackageInputModel inputModel
        )
        {
            try
            {
                var foundPackage = await Context.MealPackages
                                                
                                                .SingleOrDefaultAsync(mp => mp.ID == id);
                if (foundPackage == null)
                {
                    packageLogger.LogError(
                        MealsApiLogEvents.MealPackageNotFound,
                        "Failed to update meal package {ID} with {@InputModel}: no matching package found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["mealPackage.notFound"]);
                }

                var package = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                packageLogger.LogInformation(
                    MealsApiLogEvents.EditMealPackage,
                    "Successfully updated meal package {@Package} with {@InputModel}.",
                    package,
                    inputModel
                );

                return Ok(package);
            }
            catch (DbUpdateException e)
            {
                packageLogger.LogError(
                    MealsApiLogEvents.EditMealPackageError,
                    e,
                    "Failed to update meal package {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["mealPackage.dbFailure"]);
            }
        }
        #endregion

        #region Delete
        // DELETE /api/nutrition-support/meals/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMealAsync(int id)
        {
            try
            {
                var foundMeal = await Context.Meals.SingleOrDefaultAsync(m => m.ID == id);
                if (foundMeal == null)
                {
                    Logger.LogError(
                        MealsApiLogEvents.MealNotFound,
                        "Failed to delete meal {ID}: no matching meal found",
                        id
                    );

                    return NotFound(Localizer["meal.notFound"]);
                }

                Context.Remove(foundMeal);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    MealsApiLogEvents.DeleteMeal,
                    "Successfully deleted meal {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    MealsApiLogEvents.DeleteMealError,
                    e,
                    "Failed to delete meal {ID}.",
                    id
                );

                return ServerError(Localizer["meal.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/meals/packages/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("packages/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMealPackageAsync(
            [FromServices] ILogger<MealPackage> packageLogger,
            int id
        )
        {
            try
            {
                var foundPackage = await Context.MealPackages.SingleOrDefaultAsync(mp => mp.ID == id);
                if (foundPackage == null)
                {
                    packageLogger.LogError(
                        MealsApiLogEvents.MealPackageNotFound,
                        "Failed to delete meal package {ID}: no matching package found",
                        id
                    );

                    return NotFound(Localizer["mealPackage.notFound"]);
                }

                Context.Remove(foundPackage);
                await Context.SaveChangesAsync();

                packageLogger.LogInformation(
                    MealsApiLogEvents.DeleteMealPackage,
                    "Successfully deleted meal package {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                packageLogger.LogError(
                    MealsApiLogEvents.DeleteMealPackageError,
                    e,
                    "Failed to delete meal package {ID}.",
                    id
                );

                return ServerError(Localizer["mealPackage.deleteFailure"]);
            }
        }
        #endregion
    }
}
