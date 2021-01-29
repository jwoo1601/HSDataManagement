using HyosungManagement.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostOptions = HyosungManagement.Options.HostOptions;

namespace HyosungManagement.Configurations
{
    public class SecurityConfigManager : IConfigManager
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public SecurityConfigManager(
            IWebHostEnvironment environment,
            IConfiguration configuration
        )
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSameSiteCookies(services);
            ConfigureCors(services);
            ConfigureHsts(services);
            ConfigureForwardedHeaders(services);
        }

        public void ConfigureSameSiteCookies(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options => {
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Events.OnSigningOut += (signingOutContext) => {
                    signingOutContext.CookieOptions.SameSite = SameSiteMode.Lax;

                    return Task.CompletedTask;
                };
            });
            services.ConfigureExternalCookie(options => {
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Events.OnSigningOut += (signingOutContext) => {
                    signingOutContext.CookieOptions.SameSite = SameSiteMode.Lax;

                    return Task.CompletedTask;
                };
            });
        }

        public void ConfigureCors(IServiceCollection services)
        {
            var host = configuration.GetSection(HostOptions.Name).Get<HostOptions>();

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    if (environment.IsDevelopment())
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    }
                    else
                    {
                        builder
                            .WithOrigins(
                                $"http://{host.Address}",
                                $"https://{host.Address}"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowCredentials();
                    }
                });
            });
        }

        public void ConfigureHsts(IServiceCollection services)
        {
            if (!environment.IsDevelopment())
            {
                services.AddHsts(options => {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(365);
                });
            }
        }

        public void ConfigureForwardedHeaders(IServiceCollection services)
        {
            if (!environment.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(options => {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto;
                });
            }
        }
    }
}
