using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HyosungManagement.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace HyosungManagement
{
    public class Program
    {
        public static readonly string DevelopmentNLogConfig = "nlog.Development.config";
        public static readonly string ProductionNLogConfig = "nlog.config";
        public static readonly string SampleNLogConfig = "nlog.Sample.config";

        public static void Main(string[] args)
        {
            string nlogConfig;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == Environments.Production)
            {
                nlogConfig = ProductionNLogConfig;
            }
            else if (environment == "Sample")
            {
                nlogConfig = SampleNLogConfig;
            }
            else
            {
                nlogConfig = DevelopmentNLogConfig;
            }

            var startupLogger = NLogBuilder.ConfigureNLog(nlogConfig).GetCurrentClassLogger();
            try
            {
                startupLogger.Info("HSM Application started");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var env = services.GetRequiredService<IWebHostEnvironment>();

                    services.InjectDefaultAppData();
                    services.InjectDefaultUserData();
                    services.InjectDefaultAuthData();

                    startupLogger.Info("HSM Application environment: {Environment}", env.EnvironmentName);
                }

                host.Run();
                startupLogger.Info("HSM Application running...");
            }
            catch (Exception e)
            {
                startupLogger.Fatal(e, "HSM Application CRASHED: Exiting...");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();
            var listenPort = config.GetValue("Host:Port", 40020);

            var webHost = Host.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(webBuilder => {
                                webBuilder
                                        .UseUrls($"http://*:{listenPort}")
                                        .UseKestrel()
                                        .UseContentRoot(Directory.GetCurrentDirectory())
                                        .UseStartup<Startup>();
                            })
                            .ConfigureLogging(logging => {
                                logging.ClearProviders();
                                logging.SetMinimumLevel(LogLevel.Trace);
                            })
                            .UseNLog();

            return webHost;
        }
    }
}
