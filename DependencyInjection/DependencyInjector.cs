using HyosungManagement.Filters;
using HyosungManagement.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.DependencyInjection
{
    public static partial class DependencyInjector
    {
        public static void InjectServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration
        )
        {
            services.AddTransient<IRandomCodeGenerator, RandomPasscodeGenerator>();
            services.AddSingleton<IEmailerService, SmtpEmailerService>();
            services.AddTransient<IViewRendererService, RazorViewRendererService>();
            services.AddTransient<ClaimRequirementFilter>();
            services.AddSingleton(
                provider => new RSAKeyService(
                    environment,
                    TimeSpan.FromDays(configuration.GetValue<int>("Auth:KeyRenewalDuration"))
                )
            );
        }
    }
}
