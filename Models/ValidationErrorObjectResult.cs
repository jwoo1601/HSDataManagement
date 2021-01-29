using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public enum ResponseErrorType
    {
        General,
        Validation
    }

    public class ValidationErrorObjectResult : BadRequestObjectResult
    {
        public ValidationErrorObjectResult(ModelStateDictionary modelState)
            : base(
                  new
                  {
                      ErrorType = ResponseErrorType.Validation,
                      Errors = modelState.ToDictionary(
                          p => p.Key,
                          p => p.Value.Errors.Select(e => e.ErrorMessage)
                      )
                  }
              )
        {

        }
    }
}
