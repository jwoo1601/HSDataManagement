using HyosungManagement.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace HyosungManagement.Data
{
    public static class DbInitializer
    {
        static Customer[] Customers
            => new[]
            {
                new Customer
                {
                    Name = "이제희"
                },
                new Customer
                {
                    Name = "류홍렬"
                },
                new Customer
                {
                    Name = "황엽"
                },
                new Customer
                {
                    Name = "정연수"
                },
                new Customer
                {
                    Name = "정재철"
                },
                new Customer
                {
                    Name = "심상정",
                    IsHidden = true
                },
                new Customer
                {
                    Name = "하하"
                },
                new Customer
                {
                    Name = "정연수"
                },
            };

        static Service[] Services
            => new[]
            {
                new Service
                {
                    Name = "일반식",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "다진식",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "체중조절식",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "경관식",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "당뇨식(일)",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "당뇨식(다)",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "죽식(다)",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "죽식(일)",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "대체식",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "전환식",
                    Duration = TimeSpan.FromMinutes(60)
                },

                new Service
                {
                    Name = "생신잔치",
                    Duration = TimeSpan.FromMinutes(60)
                },

                new Service
                {
                    Name = "개별생신상",
                    Duration = TimeSpan.FromMinutes(60)
                },
                new Service
                {
                    Name = "요리프로그램",
                    Duration = TimeSpan.Zero
                },
            };

        static ServiceGroup[] ServiceGroups
            => new[]
            {
                new ServiceGroup
                {
                    Name = ""
                }
            };

        public static IServiceProvider InjectDefaultAppData(this IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                context.AddData();
            }
            catch (Exception e)
            {
                services.GetRequiredService<ILogger<Program>>()
                        .LogError(e, "An error occured while seeding the main database with default data.");
            }

            return services;
        }

        public static AppDbContext AddData(this AppDbContext context)
        {
            if (!context.Customers.Any())
            {
                foreach (var c in Customers)
                {
                    context.Customers.Add(c);
                };
                context.SaveChanges();
            }

            if (!context.ServiceGroups.Any())
            {
                foreach (var sg in ServiceGroups)
                {
                    context.ServiceGroups.Add(sg);
                }
                context.SaveChanges();
            }

            if (!context.Services.Any())
            {
                foreach (var s in Services)
                {
                    context.Services.Add(s);
                };
                context.SaveChanges();
            }

            return context;
        }
    }
}
