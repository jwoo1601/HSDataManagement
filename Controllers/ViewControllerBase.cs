using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HyosungManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HyosungManagement.Controllers
{
    public abstract class ViewControllerBase : Controller
    {
        protected IActionResult Success(string title, string message)
        {
            return Success(
                new SuccessViewModel
                {
                    Title = title,
                    Message = message
                }
            );
        }

        protected IActionResult Success(SuccessViewModel viewModel)
        {
            return View(
                "Success",
                viewModel
            );
        }

        protected IActionResult Error(string message)
        {
            return Error(
                new ErrorViewModel
                {
                    Message = message
                }
            );
        }

        protected IActionResult Error(string title, string message)
        {
            return Error(
                new ErrorViewModel
                {
                    Title = title,
                    Message = message
                }
            );
        }

        protected IActionResult Error(ErrorViewModel viewModel)
        {
            return View(
                "Error",
                viewModel
            );
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            //var defaultCulture = CultureInfo.GetCultureInfo("ko-KR");
            //Thread.CurrentThread.CurrentCulture = defaultCulture;
            //Thread.CurrentThread.CurrentUICulture = defaultCulture;
        }
    }
}
