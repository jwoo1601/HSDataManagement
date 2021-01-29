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
using HyosungManagement.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HyosungManagement.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [RoleRequirement("Master")]
    [Route("admin")]
    public class AdminController : ViewControllerBase
    {
        [HttpGet("router-info")]
        public IActionResult RouterInfo()
        {
            return View();
        }
    }
}
