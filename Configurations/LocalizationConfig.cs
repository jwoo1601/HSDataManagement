using HyosungManagement.InputModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public class LocalizationConfig : IServiceConfig
    {
        public static readonly string DefaultCulture = "ko-KR";
        public static readonly string[] DefaultSupportedCultures
            = new[] { "en-US", "ko-KR" };

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public LocalizationConfig(
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJsonLocalization(
                options => {
                    options.ResourcesPath = "Resources";
                }
            );

            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization(
                        options => {
                            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
                        }
                    );
        }

        public RequestLocalizationOptions GetLocalizationOptions()
        {
            return new RequestLocalizationOptions()
            .SetDefaultCulture(
                Configuration.GetValue("i18n:DefaultCulture", DefaultCulture)
            )
            .AddSupportedCultures(
                Configuration.GetValue(
                    "i18n:SupportedCultures",
                    DefaultSupportedCultures
                )
            );
        }
    }
}
