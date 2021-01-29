using HyosungManagement.Options;
using HyosungManagement.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public class OptionsConfigManager : IConfigManager
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public OptionsConfigManager(
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailerOptions>(
                Configuration.GetSection(EmailerOptions.Emailer)
            );

            services.Configure<ViewRendererOptions>(
                Configuration.GetSection(ViewRendererOptions.Name)
            );

            services.Configure<ViewPdfRendererOptions>(
                Configuration.GetSection(ViewPdfRendererOptions.Name)
            );

            services.Configure<AccountOptions>(
                Configuration.GetSection(AccountOptions.Name)
            );

            services.Configure<HostOptions>(
                Configuration.GetSection(HostOptions.Name)
            );

            services.Configure<AuthOptions>(
                Configuration.GetSection(AuthOptions.Name)
            );
        }
    }
}
