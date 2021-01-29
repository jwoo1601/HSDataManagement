using HyosungManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public class ApiConfigManager : IConfigManager
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public ApiConfigManager(
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .ConfigureApiBehaviorOptions(options => {
                    options.SuppressMapClientErrors = true;
                    options.InvalidModelStateResponseFactory =
                        (context) => new ValidationErrorObjectResult(context.ModelState);
                });

            services.AddAntiforgery(options => {
                options.Cookie.Name = "X-CSRF-TOKEN";
                options.HeaderName = "X-CSRF-TOKEN";
            });
        }
    }
}
