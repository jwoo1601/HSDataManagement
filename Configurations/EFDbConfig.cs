using HyosungManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HyosungManagement.Configurations
{
    public class EFDbConfig : IServiceConfig
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public EFDbConfig(
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
                .AddDbContext<AppDbContext>(
                    options => {
                        options.UseSqlServer(
                            Configuration.GetConnectionString("Default")
                        );

                        options.UseLazyLoadingProxies();
                    }
                )
                .AddDbContext<UserDbContext>(options => {
                    options.UseSqlServer(
                        Configuration.GetConnectionString("User")
                    );

                    options.UseLazyLoadingProxies();
                })
                .AddDbContext<AuthDbContext>(options => {
                    options.UseSqlServer(
                        Configuration.GetConnectionString("Auth")
                    );
                });
        }
    }
}
