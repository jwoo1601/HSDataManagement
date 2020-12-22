using HyosungManagement.Filters;
using HyosungManagement.Services;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.DependencyInjection
{
    public static partial class DependencyInjector
    {
        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<IRandomCodeGenerator, RandomPasscodeGenerator>();
            services.AddSingleton<IEmailerService, SmtpEmailerService>();
            services.AddTransient<IViewRendererService, RazorViewRendererService>();
            services.AddTransient<ClaimRequirementFilter>();
        }
    }
}
