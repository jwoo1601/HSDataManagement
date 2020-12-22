using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyosungManagement.Configurations;
using HyosungManagement.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using Hangfire;
using Microsoft.AspNetCore.SpaServices.Webpack;
using HyosungManagement.Filters;
using HyosungManagement.Extensions;
using VueCliMiddleware;
using Microsoft.AspNetCore.SpaServices;

namespace HyosungManagement
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        public EFDbConfig DbConfig { get; }
        public AuthConfig AuthConfig { get; }
        public LocalizationConfig LocalizationConfig { get; }
        public ServiceConfig ServiceConfig { get; }
        public ApiConfig ApiConfig { get; }

        private readonly static string allowSelfOrigins = "allowSelfOrigins";

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment hostEnvironment
        )
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
            DbConfig = new EFDbConfig(configuration, hostEnvironment);
            AuthConfig = new AuthConfig(configuration, hostEnvironment);
            LocalizationConfig = new LocalizationConfig(configuration, hostEnvironment);
            ServiceConfig = new ServiceConfig(configuration, hostEnvironment);
            ApiConfig = new ApiConfig(configuration, hostEnvironment);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjector.InjectServices(services);

            ServiceConfig.ConfigureServices(services);
            DbConfig.ConfigureServices(services);
            AuthConfig.ConfigureServices(services);
            LocalizationConfig.ConfigureServices(services);
            ApiConfig.ConfigureServices(services);

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

            services.AddCors(options => {
                var baseUrl = Configuration["Host:BaseUrl"];
                var port = Configuration["Host:Port"];
                var serverUrl = $"{baseUrl}:{port}";

                options.AddPolicy(allowSelfOrigins, builder => {
                    if (HostEnvironment.IsDevelopment())
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    }
                    else
                    {
                        builder
                            .WithOrigins(
                                $"http://*.{serverUrl}",
                                $"https://*.{serverUrl}"

                            )
                            .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete, HttpMethods.Patch)
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowCredentials();
                    }
                });
            });

            if (HostEnvironment.IsProduction())
            {
                services.AddHsts(options => {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(365);
                });

                services.AddHttpsRedirection(options => {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 5001;
                });
            }

            services.AddHangfire(
                options => options.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"))
            );
            services.AddHangfireServer();

            services.AddSpaStaticFiles(options => options.RootPath = "client-app/dist");
            services.AddControllersWithViews()
                    .AddJsonConfig();

            if (HostEnvironment.IsProduction())
            {
                services.Configure<ForwardedHeadersOptions>(options => {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
                app.UseExceptionHandler("/home/error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRequestLocalization(
                LocalizationConfig.GetLocalizationOptions()
            );

            app.UseRouting();
            app.UseCors(allowSelfOrigins);

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions
                {
                    DashboardTitle = "HSM Background Jobs Dashboard",
                    AppPath = "/",
                    Authorization = new[] { new HangfireAuthorizationFilter() },
                    //PrefixPath = "/master"
                }
            );

            app.UseEndpoints(endpoints => {
                //endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(
                    name: "nutrition_support",
                    areaName: "nutrition-support",
                    pattern: "{area}/{controller}/{action}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}"
                );

                //if (env.IsDevelopment())
                //{
                //    endpoints.MapToVueCliProxy(
                //        "{*path}",
                //        new SpaOptions { SourcePath = "client-app" },
                //        npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                //        //regex: "Build complete.",
                //        https: env.IsProduction(),
                //        forceKill: true
                //    );
                //}
            });
        }
    }
}
