using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HyosungManagement.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using HyosungManagement.ViewModels;
using Microsoft.Extensions.Localization;

namespace HyosungManagement.Controllers
{
    public class HomeController : ViewControllerBase
    {
        ILogger<HomeController> Logger { get; }
        IIdentityServerInteractionService InteractionService { get; }
        IWebHostEnvironment HostEnvironment { get; }
        IStringLocalizer<HomeController> Localizer { get; }

        public HomeController(
            ILogger<HomeController> logger,
            IIdentityServerInteractionService interactionService,
            IWebHostEnvironment hostEnvironment,
            IStringLocalizer<HomeController> localizer
        )
        {
            Logger = logger;
            InteractionService = interactionService;
            HostEnvironment = hostEnvironment;
            Localizer = localizer;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("error", Name = "Error")]
        public async Task<IActionResult> ErrorAsync(
            [FromQuery(Name = "auth_error_id")] string authErrorID
        )
        {
            var viewModel = new ErrorViewModel();

            if (string.IsNullOrWhiteSpace(authErrorID))
            {
                viewModel.Message = Localizer["error.message.unknown"];
            }
            else
            {
                // retrieve error details from identityserver
                var id4ErrorMessage = await InteractionService.GetErrorContextAsync(authErrorID);
                if (id4ErrorMessage != null)
                {
                    viewModel.Title = Localizer["error.title.auth", id4ErrorMessage.RequestId];
                    viewModel.Message = Localizer["error.message.auth", id4ErrorMessage.Error];
                    viewModel.AdditionalLinkText = Localizer["error.linkText"];
                    viewModel.AdditionalLinkUrl = id4ErrorMessage.RedirectUri;

                    if (HostEnvironment.IsDevelopment())
                    {
                        viewModel.Message = $"{viewModel.Message} {id4ErrorMessage.ErrorDescription}";
                    }
                }
            }

            return Error(viewModel);
        }
    }
}
