using HyosungManagement.InputModels;
using HyosungManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Created(object obj)
        {
            return StatusCode(
                StatusCodes.Status201Created,
                obj
            );
        }

        private object CreateErrorResponse(ResponseErrorType errorType, string message)
        {
            return new { ErrorType = errorType, Message = message };
        }

        protected IActionResult BadRequest(string message)
        {
            return BadRequest(
                CreateErrorResponse(ResponseErrorType.General, message)
            );
        }

        protected IActionResult NotFound(string message)
        {
            return NotFound(
                CreateErrorResponse(ResponseErrorType.General, message)
            );
        }

        protected IActionResult ServerError(string message)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                CreateErrorResponse(ResponseErrorType.General, message)
            );
        }

        protected IActionResult ValidationError(ModelStateDictionary modelState = null)
        {
            return new ValidationErrorObjectResult(modelState ?? ModelState);
        }

        protected async Task<TEntity> SaveModel<TInputModel, TDbContext, TEntity, TKey>(
            TDbContext context,
            TInputModel inputModel,
            TKey? key = null
        )
            where TInputModel : IEntityInputModel<TDbContext, TEntity, TKey>
            where TKey : struct
        {
            return await inputModel.SaveAsEntityAsync(key, context, HttpContext.RequestServices);
        }
    }
}
