using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyosungManagement.Configurations;
using HyosungManagement.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Hangfire;
using HyosungManagement.Filters;

namespace HyosungManagement
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        public DbConfigManager DbConfig { get; }
        public AuthConfigManager AuthConfig { get; }
        public LocalizationConfig LocalizationConfig { get; }
        public OptionsConfigManager OptionsConfig { get; }
        public ApiConfigManager ApiConfig { get; }
        private readonly SecurityConfigManager securityConfig;

        private readonly static string allowSelfOrigins = "allowSelfOrigins";

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment hostEnvironment
        )
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
            DbConfig = new DbConfigManager(configuration, hostEnvironment);
            AuthConfig = new AuthConfigManager(configuration, hostEnvironment);
            LocalizationConfig = new LocalizationConfig(configuration, hostEnvironment);
            OptionsConfig = new OptionsConfigManager(configuration, hostEnvironment);
            ApiConfig = new ApiConfigManager(configuration, hostEnvironment);
            securityConfig = new SecurityConfigManager(hostEnvironment, configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjector.InjectServices(services, HostEnvironment, Configuration);

            OptionsConfig.ConfigureServices(services);
            DbConfig.ConfigureServices(services);
            AuthConfig.ConfigureServices(services);
            LocalizationConfig.ConfigureServices(services);
            ApiConfig.ConfigureServices(services);
            securityConfig.ConfigureServices(services);

            services.AddHangfire(
                options => options.UseNLogLogProvider()
                                .UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"))
            );
            services.AddHangfireServer();

            services.AddControllersWithViews()
                    .AddJsonConfig();

            services.AddSpaStaticFiles(options => {
                options.RootPath = "client-app/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders();
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRequestLocalization(
                LocalizationConfig.GetLocalizationOptions()
            );

            app.UseCors(allowSelfOrigins);
            app.UseRouting();

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
                endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(
                    name: "nutrition_support",
                    areaName: "nutrition-support",
                    pattern: "{area}/{controller}/{action}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}"
                );

                //endpoints.MapFallbackToController("Index", "Home");

                //if (env.IsDevelopment())
                //{
                //endpoints.MapToVueCliProxy(
                //    "{*path}",
                //    new SpaOptions { SourcePath = "client-app" },
                //    npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                //    regex: "Build complete.",
                //    https: env.IsProduction(),
                //    forceKill: true
                //);
                //}
            });

            app.UseSpa(spa => {
                //if (env.IsDevelopment())
                //{
                //    spa.Options.SourcePath = "client-app";
                //}
                //else
                //{
                spa.Options.SourcePath = "dist";
                //}

                //if (env.IsDevelopment())
                //{
                //    spa.UseVueCli(
                //        npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                //        regex: "Build complete.",
                //        https: env.IsProduction(),
                //        forceKill: true
                //    );
                //}
            });
        }
    }
}
