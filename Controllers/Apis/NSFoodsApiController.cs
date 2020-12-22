using System;
using System.Collections.Generic;
using System.Data.Common;
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
    [Area("nutrition-support")]
    [Route("api/[area]/foods")]
    [ApiController]
    public class NSFoodsApiController : ApiControllerBase
    {
        AppDbContext Context { get; }
        ILogger<Food> Logger { get; }
        IStringLocalizer<NSFoodsApiController> Localizer { get; }

        public NSFoodsApiController(
            AppDbContext context,
            ILogger<Food> logger,
            IStringLocalizer<NSFoodsApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
        }

        #region GetAll-
        // GET /api/nutrition-support/foods
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodListAsync()
        {
            var list = await Context.Foods
                                            .ToListAsync();

            Logger.LogInformation(
                FoodsApiLogEvents.GetAllFoodList,
                "Successfully retrieved list of food - numFood: {numFood}.",
                list.Count
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/foods/categories
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("categories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodCategoriesAsync(
            [FromServices] ILogger<FoodCategory> categoryLogger
        )
        {
            var list = await Context.FoodCategories
                                                    .ToListAsync();
            categoryLogger.LogInformation(
                FoodsApiLogEvents.GetAllFoodCategories,
                "Successfully retrieved list of food categories - num: {num}.",
                list.Count
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/foods/ingredients
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("ingredients")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodIngredientsAsync(
            [FromServices] ILogger<FoodIngredient> ingredientLogger
        )
        {
            var list = await Context.FoodIngredients
                                                    .ToListAsync();

            ingredientLogger.LogInformation(
                FoodsApiLogEvents.GetAllFoodIngredients,
                "Successfully retrieved list of food ingredients - num: {num}.",
                list.Count
            );

            return Ok(list);
        }

        // GET /api/nutrition-support/foods/ingredients/categories
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("ingredients/categories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoodIngredientCategoriesAsync(
            [FromServices] ILogger<FoodIngredientCategory> categoryLogger
        )
        {
            var list = await Context.FoodIngredientCategories
                                                            .ToListAsync();

            categoryLogger.LogInformation(
                FoodsApiLogEvents.GetAllFoodIngredientCategories,
                "Successfully retrieved list of food ingredient categories - num: {num}.",
                list.Count
            );

            return Ok(list);
        }
        #endregion

        #region Get-ById
        // GET /api/nutrition-support/foods/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoodByIdAsync(int id)
        {
            var foundFood = await Context.Foods
                                                .SingleOrDefaultAsync(f => f.ID == id);
            if (foundFood == null)
            {
                Logger.LogError(
                    FoodsApiLogEvents.FoodNotFound,
                    "Failed to retrieve food with id {ID}: food not found.",
                    id
                );

                return NotFound(Localizer["food.notFound"]);
            }

            Logger.LogInformation(
                FoodsApiLogEvents.GetFoodByID,
                "Successfully retrieved food {@Food}.",
                foundFood
            );

            return Ok(foundFood);
        }

        // GET /api/nutrition-support/foods/categories/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoodCategoryByIdAsync(
            [FromServices] ILogger<FoodCategory> categoryLogger,
            int id
        )
        {
            var foundCategory = await Context.FoodCategories
                                            
                                            .SingleOrDefaultAsync(f => f.ID == id);
            if (foundCategory == null)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.FoodCategoryNotFound,
                    "Failed to retrieve food category with id {ID}: food category not found.",
                    id
                );

                return NotFound(Localizer["foodCategory.notFound"]);
            }

            categoryLogger.LogInformation(
                FoodsApiLogEvents.GetFoodCategoryByID,
                "Successfully retrieved food category {@Category}.",
                foundCategory
            );

            return Ok(foundCategory);
        }

        // GET /api/nutrition-support/foods/ingredients/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("ingredients/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoodIngredientByIdAsync(
            [FromServices] ILogger<FoodCategory> ingredientLogger,
            int id
        )
        {
            var foundItem = await Context.FoodIngredients
                                            
                                            .SingleOrDefaultAsync(f => f.ID == id);
            if (foundItem == null)
            {
                ingredientLogger.LogError(
                    FoodsApiLogEvents.FoodIngredientNotFound,
                    "Failed to retrieve food ingredient with id {ID}: food ingredient not found.",
                    id
                );

                return NotFound(Localizer["foodIngredient.notFound"]);
            }

            ingredientLogger.LogInformation(
                FoodsApiLogEvents.GetFoodIngredientByID,
                "Successfully retrieved food ingredient {@Ingredient}.",
                foundItem
            );

            return Ok(foundItem);
        }

        // GET /api/nutrition-support/foods/ingredients/categories/{id}
        [RoleRequirement("User", "Admin", "Master")]
        [HttpGet("ingredients/categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoodIngredientCategoryByIdAsync(
            [FromServices] ILogger<FoodIngredientCategory> categoryLogger,
            int id
        )
        {
            var foundCategory = await Context.FoodIngredientCategories
                                            
                                            .SingleOrDefaultAsync(f => f.ID == id);
            if (foundCategory == null)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.FoodIngredientCategoryNotFound,
                    "Failed to retrieve food ingredient category with id {ID}: food ingredient category not found.",
                    id
                );

                return NotFound(Localizer["foodIngredientCategory.notFound"]);
            }

            categoryLogger.LogInformation(
                FoodsApiLogEvents.GetFoodIngredientCategoryByID,
                "Successfully retrieved food ingredient category {@Category}.",
                foundCategory
            );

            return Ok(foundCategory);
        }
        #endregion

        #region Add
        // POST /api/nutrition-support/foods
        [RoleRequirement("Admin", "Master")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFoodAsync(
            [FromBody] FoodInputModel inputModel
        )
        {
            try
            {
                var foundCategory = await Context.FoodCategories
                                                
                                                .SingleOrDefaultAsync(fc => fc.ID == inputModel.Category);
                if (foundCategory == null)
                {
                    Logger.LogError(
                        FoodsApiLogEvents.FoodCategoryNotFound,
                        "Failed to add food with {@InputModel}: no matching category found",
                        inputModel
                    );

                    return NotFound(Localizer["foodCategory.notFound"]);
                }

                var food = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                Logger.LogInformation(
                    FoodsApiLogEvents.AddFood,
                    "Successfully added new food {@Food} with {@InputModel}.",
                    food,
                    inputModel
                );

                return Created(food);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    FoodsApiLogEvents.AddFoodError,
                    e,
                    "Failed to add new food with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["food.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/foods/categories
        [RoleRequirement("Admin", "Master")]
        [HttpPost("categories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFoodCategoryAsync(
            [FromServices] ILogger<FoodCategory> categoryLogger,
            [FromBody] FoodCategoryInputModel inputModel
        )
        {
            try
            {
                var category = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.AddFoodCategory,
                    "Successfully added new food category {@Category} with {@InputModel}.",
                    category,
                    inputModel
                );

                return Created(category);
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.AddFoodCategoryError,
                    e,
                    "Failed to add food category with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["foodCategory.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/foods/ingredients
        [RoleRequirement("Admin", "Master")]
        [HttpPost("ingredients")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFoodIngredientAsync(
            [FromServices] ILogger<FoodIngredient> ingredientLogger,
            [FromBody] FoodIngredientInputModel inputModel
        )
        {
            try
            {
                var foundCategory = await Context.FoodIngredientCategories
                                                
                                                .SingleOrDefaultAsync(fc => fc.ID == inputModel.Category);
                if (foundCategory == null)
                {
                    ingredientLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientCategoryNotFound,
                        "Failed to add food ingredient with {@InputModel}: no matching category found",
                        inputModel
                    );

                    return NotFound(Localizer["foodIngredientCategory.notFound"]);
                }

                var ingredient = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);

                ingredientLogger.LogInformation(
                    FoodsApiLogEvents.AddFoodIngredient,
                    "Successfully added food ingredient {@Ingredient} with {@InputModel}.",
                    ingredient,
                    inputModel
                );

                return Created(ingredient);
            }
            catch (DbUpdateException e)
            {
                ingredientLogger.LogError(
                    FoodsApiLogEvents.AddFoodIngredientError,
                    e,
                    "Failed to add food ingredient with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["foodIngredient.dbFailure"]);
            }
        }

        // POST /api/nutrition-support/foods/ingredients/categories
        [RoleRequirement("Admin", "Master")]
        [HttpPost("ingredients/categories")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFoodIngredientCategoryAsync(
            [FromServices] ILogger<FoodIngredientCategory> categoryLogger,
            [FromBody] FoodIngredientCategoryInputModel inputModel
        )
        {
            try
            {
                var category = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.AddFoodIngredientCategory,
                    "Successfully added food ingredient category {@Category} with {@InputModel}.",
                    category,
                    inputModel
                );

                return Created(category);
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.AddFoodIngredientCategoryError,
                    e,
                    "Failed to add food ingredient category with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["foodIngredientCategory.dbFailure"]);
            }
        }
        #endregion

        #region Edit
        // PUT /api/nutrition-support/foods/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditFoodAsync(
            int id,
            [FromBody] FoodInputModel inputModel
        )
        {
            try
            {
                var foundFood = await Context.Foods.SingleOrDefaultAsync(f => f.ID == id);
                if (foundFood == null)
                {
                    Logger.LogError(
                        FoodsApiLogEvents.FoodNotFound,
                        "Failed to update food {ID} with {@InputModel}: no matching food found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["food.notFound"]);
                }

                var foundCategory = await Context.FoodCategories
                                                
                                                .SingleOrDefaultAsync(fc => fc.ID == inputModel.Category);
                if (foundCategory == null)
                {
                    Logger.LogError(
                        FoodsApiLogEvents.FoodCategoryNotFound,
                        "Failed to update food {ID} with {@InputModel}: no matching category found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["foodCategory.notFound"]);
                }

                var food = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                //foundFood.Name = inputModel.Name;
                //foundFood.Category = foundCategory;
                //foundFood.Note = inputModel.Note;

                //Context.RemoveRange(
                //    foundFood.IngredientAssignments
                //);
                //await Context.SaveChangesAsync();

                //await AddExistingIngredientsAsync(foundFood, inputModel.Ingredients);

                //var updatedFood = await Context.Foods
                //                                .Include(f => f.Category)
                //                                .Include(f => f.IngredientAssignments)
                //                                .ThenInclude(a => a.Ingredient)
                //                                
                //                                .SingleOrDefaultAsync(f => f.ID == id);

                Logger.LogInformation(
                    FoodsApiLogEvents.EditFood,
                    "Successfully updated food {@Food} with {@InputModel}.",
                    food,
                    inputModel
                );

                return Ok(food);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    FoodsApiLogEvents.EditFoodError,
                    e,
                    "Failed to update food {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["food.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/foods/categories/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditFoodCategoryAsync(
            [FromServices] ILogger<FoodCategory> categoryLogger,
            int id,
            [FromBody] FoodCategoryInputModel inputModel
        )
        {
            try
            {
                var foundCategory = await Context.FoodCategories
                                                
                                                .SingleOrDefaultAsync(fc => fc.ID == id);
                if (foundCategory == null)
                {
                    categoryLogger.LogError(
                        FoodsApiLogEvents.FoodCategoryNotFound,
                        "Failed to update food category {ID} with {@InputModel}: no matching category found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["foodCategory.notFound"]);
                }

                var category = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.EditFoodCategory,
                    "Successfully updated food category {@Category} with {@InputModel}.",
                    category,
                    inputModel
                );

                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.EditFoodCategoryError,
                    e,
                    "Failed to update food category {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["foodCategory.dbFailure"]);
            }
        }

        // PUT /api/nutrition-support/foods/ingredients/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("ingredients/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditFoodIngredientAsync(
            [FromServices] ILogger<FoodIngredient> ingredientLogger,
            int id,
            [FromBody] FoodIngredientInputModel inputModel
        )
        {
            try
            {
                var foundIngredient = await Context.FoodIngredients.SingleOrDefaultAsync(ig => ig.ID == id);
                if (foundIngredient == null)
                {
                    ingredientLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientNotFound,
                        "Failed to update food ingredient {ID} with {@InputModel}: no matching ingredient found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["foodIngredient.notFound"]);
                }

                var foundCategory = await Context.FoodIngredientCategories
                                                
                                                .SingleOrDefaultAsync(igc => igc.ID == inputModel.Category);
                if (foundCategory == null)
                {
                    ingredientLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientCategoryNotFound,
                        "Failed to update food ingredient {ID} with {@InputModel}: no matching category found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["foodIngredientCategory.notFound"]);
                }

                var ingredient = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                ingredientLogger.LogInformation(
                    FoodsApiLogEvents.EditFoodIngredient,
                    "Successfully updated food ingredient {@Ingredient} with {@InputModel}.",
                    ingredient,
                    inputModel
                );

                return Ok(ingredient);
            }
            catch (DbUpdateException e)
            {
                ingredientLogger.LogError(
                    FoodsApiLogEvents.EditFoodIngredientError,
                    e,
                    "Failed to update food ingredient {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["foodIngredient.dbFailure"]);
            }

            //try
            //{
            //    var foundIngredient = await Context.FoodIngredients
            //                                    .SingleOrDefaultAsync(
            //                                        fi => fi.ID == id
            //                                    );
            //    if (foundIngredient == null)
            //    {
            //        return NotFound(new { Message = "No matching food ingredient found." });
            //    }

            //    foundIngredient.Name = inputModel.Name;
            //    foundIngredient.Origin = inputModel.Origin;
            //    await Context.SaveChangesAsync();

            //    var updatedIngredient = await Context.FoodIngredients
            //                                    
            //                                    .SingleOrDefaultAsync(fi => fi.ID == id);
            //    return Ok(updatedIngredient);
            //}
            //catch (DbUpdateException e)
            //{
            //    var errorMessage = $"Failed to update food ingredient {id}.";
            //    ingredientLogger.LogError(e, errorMessage);

            //    return ServerError(errorMessage);
            //}
        }

        // PUT /api/nutrition-support/foods/ingredients/categories/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpPut("ingredients/categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditFoodIngredientCategoryAsync(
            [FromServices] ILogger<FoodIngredientCategory> categoryLogger,
            int id,
            [FromBody] FoodIngredientCategoryInputModel inputModel
        )
        {
            try
            {
                var foundCategory = await Context.FoodIngredientCategories
                                                    
                                                    .SingleOrDefaultAsync(igc => igc.ID == id);
                if (foundCategory == null)
                {
                    categoryLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientCategoryNotFound,
                        "Failed to update food ingredient category {ID} with {@InputModel}: no matching category found",
                        id,
                        inputModel
                    );

                    return NotFound(Localizer["foodIngredientCategory.notFound"]);
                }

                var category = await inputModel.SaveAsEntityAsync(
                    id,
                    Context,
                    HttpContext.RequestServices
                );

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.EditFoodIngredientCategory,
                    "Successfully updated food ingredient category {@Category} with {@InputModel}.",
                    category,
                    inputModel
                );

                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.EditFoodIngredientCategoryError,
                    e,
                    "Failed to update food ingredient category {ID} with {@InputModel}.",
                    id,
                    inputModel
                );

                return ServerError(Localizer["foodIngredientCategory.dbFailure"]);
            }
        }
        #endregion

        #region Delete
        // DELETE /api/nutrition-support/foods/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoodAsync(int id)
        {
            try
            {
                var foundFood = await Context.Foods.SingleOrDefaultAsync(f => f.ID == id);
                if (foundFood == null)
                {
                    Logger.LogError(
                        FoodsApiLogEvents.FoodNotFound,
                        "Failed to delete food {ID}: no matching food found",
                        id
                    );

                    return NotFound(Localizer["food.notFound"]);
                }

                Context.Remove(foundFood);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    FoodsApiLogEvents.DeleteFood,
                    "Successfully deleted food {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    FoodsApiLogEvents.DeleteFoodError,
                    e,
                    "Failed to delete food {ID}.",
                    id
                );

                return ServerError(Localizer["food.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/foods/categories/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoodCategoryAsync(
            [FromServices] ILogger<FoodCategory> categoryLogger,
            int id
        )
        {
            try
            {
                var foundCategory = await Context.FoodCategories.SingleOrDefaultAsync(fc => fc.ID == id);
                if (foundCategory == null)
                {
                    categoryLogger.LogError(
                        FoodsApiLogEvents.FoodCategoryNotFound,
                        "Failed to delete food category {ID}: no matching category found",
                        id
                    );

                    return NotFound(Localizer["foodCategory.notFound"]);
                }

                Context.Remove(foundCategory);
                await Context.SaveChangesAsync();

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.DeleteFoodCategory,
                    "Successfully deleted food category {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.DeleteFoodCategoryError,
                    e,
                    "Failed to delete food category {ID}.",
                    id
                );

                return ServerError(Localizer["foodCategory.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/foods/ingredients/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("ingredients/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoodIngredientAsync(
            [FromServices] ILogger<FoodIngredient> ingredientLogger,
            int id
        )
        {
            try
            {
                var foundIngredient = await Context.FoodIngredients.SingleOrDefaultAsync(ig => ig.ID == id);
                if (foundIngredient == null)
                {
                    ingredientLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientNotFound,
                        "Failed to delete food ingredient {ID}: no matching ingredient found",
                        id
                    );

                    return NotFound(Localizer["foodIngredient.notFound"]);
                }

                Context.Remove(foundIngredient);
                await Context.SaveChangesAsync();

                ingredientLogger.LogInformation(
                    FoodsApiLogEvents.DeleteFoodIngredient,
                    "Successfully deleted food ingredient {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                ingredientLogger.LogError(
                    FoodsApiLogEvents.DeleteFoodIngredientError,
                    e,
                    "Failed to delete food ingredient {ID}.",
                    id
                );

                return ServerError(Localizer["foodIngredient.deleteFailure"]);
            }
        }

        // DELETE /api/nutrition-support/foods/ingredients/categories/{id}
        [RoleRequirement("Admin", "Master")]
        [HttpDelete("ingredients/categories/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFoodIngredientCategoryAsync(
            [FromServices] ILogger<FoodIngredientCategory> categoryLogger,
            int id
        )
        {
            try
            {
                var foundCategory = await Context.FoodIngredientCategories.SingleOrDefaultAsync(igc => igc.ID == id);
                if (foundCategory == null)
                {
                    categoryLogger.LogError(
                        FoodsApiLogEvents.FoodIngredientCategoryNotFound,
                        "Failed to delete food ingredient category {ID}: no matching category found",
                        id
                    );

                    return NotFound(Localizer["foodIngredientCategory.notFound"]);
                }

                Context.Remove(foundCategory);
                await Context.SaveChangesAsync();

                categoryLogger.LogInformation(
                    FoodsApiLogEvents.DeleteFoodIngredientCategory,
                    "Successfully deleted food ingredient category {ID}.",
                    id
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                categoryLogger.LogError(
                    FoodsApiLogEvents.DeleteFoodIngredientCategoryError,
                    e,
                    "Failed to delete food ingredient category {ID}.",
                    id
                );

                return ServerError(Localizer["foodIngredientCategory.deleteFailure"]);
            }
        }
        #endregion

        private async Task AddExistingIngredientsAsync(Food food, IEnumerable<int> ingredientIDs)
        {
            var ingredientList = await Context.FoodIngredients
                                                
                                                .ToListAsync();

            var matchingIngredients = ingredientIDs.Where(
                id => ingredientList.Any(fi => fi.ID == id)
            );
            foreach (var ingredient in matchingIngredients)
            {
                await Context.FoodIngredientAssignments.AddAsync(
                    new FoodIngredientAssignment
                    {
                        FoodID = food.ID,
                        FoodIngredientID = ingredient
                    }
                );
            }
            await Context.SaveChangesAsync();
        }
    }
}
