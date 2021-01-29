using HyosungManagement.Data;
using HyosungManagement.Models.Identity;
using HyosungManagement.Options;
using HyosungManagement.Services;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public class AuthConfigManager : IConfigManager
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public AuthConfigManager(
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var auth = Configuration.GetSection(AuthOptions.Name).Get<AuthOptions>();

            services.AddIdentity<HSMUser, HSMRole>()
                    .AddEntityFrameworkStores<UserDbContext>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<HSMUserClaimsPrincipalFactory>();

            var identity4Builder = services
                        .AddIdentityServer(options => {
                            options.IssuerUri = auth.Issuer;

                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseInformationEvents = true;
                            options.Events.RaiseFailureEvents = true;
                            options.Events.RaiseSuccessEvents = true;

                            options.UserInteraction.LoginUrl = "/account/login";
                            options.UserInteraction.LoginReturnUrlParameter = "redirect_url";
                            options.UserInteraction.LogoutUrl = "/account/logout";
                            options.UserInteraction.LogoutIdParameter = "logout_id";
                            options.UserInteraction.ErrorUrl = "/error";
                            options.UserInteraction.ErrorIdParameter = "auth_error_id";

                            options.Endpoints.EnableDeviceAuthorizationEndpoint = false;

                            options.Authentication.CookieSameSiteMode = SameSiteMode.Lax;
                            options.Authentication.CheckSessionCookieSameSiteMode = SameSiteMode.Lax;
                        })
                        .AddAspNetIdentity<HSMUser>()
                        .AddConfigurationStore<AuthDbContext>(options => {
                            options.ConfigureDbContext = builder => {
                                builder.UseSqlServer(
                                    Configuration.GetConnectionString("Auth")
                                );
                            };
                        })
                        .AddOperationalStore<AuthDbContext>(options => {
                            options.ConfigureDbContext = builder => {
                                builder.UseSqlServer(
                                    Configuration.GetConnectionString("Auth")
                                );
                            };
                        })
                        .AddProfileService<HSMProfileService>();

            if (Environment.IsDevelopment())
            {
                identity4Builder.AddDeveloperSigningCredential();
            }
            else
            {
                identity4Builder.AddSigningCredential(
                    new X509Certificate2(
                        auth.CertificatePath,
                        auth.EncryptKey,
                        X509KeyStorageFlags.MachineKeySet |
                        X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable
                    )
                );
                //identity4Builder.AddSigningCredential(
                //    new X509SigningCredentials(
                //        new X509Certificate2(
                //            Path.Combine(
                //                Directory.GetCurrentDirectory(),
                //                Path.GetRelativePath(
                //                    Directory.GetCurrentDirectory(),
                //                    Configuration["Auth:SigningCertificatePath"]
                //                )
                //            )
                //        )
                //    )
                //);
            }

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Authorization server configurations
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    , options => {
            //    //options.DefaultAuthenticateScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
            //    //options.DefaultChallengeScheme = ;
            //    //options.DefaultForbidScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
            //    //options.DefaultSignInScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
            //    //options.DefaultSignOutScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options => {
                    options.LoginPath = "/account/login";
                    options.AccessDeniedPath = "/account/access-denied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    // Only allow the authentication cookie to be send to the server from first-party app
                    options.Cookie.SameSite = SameSiteMode.Lax;
                })
            .AddJwtBearer(options => {
                options.Authority = auth.Authority;
                options.ClaimsIssuer = auth.Issuer;
                options.Audience = auth.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidTypes = new[] { "at+jwt" },
                    ValidIssuer = auth.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = auth.Audience,
                    ValidateAudience = true,
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };

                if (Environment.IsDevelopment())
                {
                    options.RequireHttpsMetadata = false;
                }
            });

            // this includes both JwtBearer and OAuth2Introspection
            //.AddIdentityServerAuthentication(
            //    options => {
            //        options.ApiName = IdentityServerConstants.LocalApi.ScopeName;
            //        options.Authority = $"http://{Configuration["Host:BaseUrl"]}:{Configuration["Host:Port"]}";

            //        //options.TokenValidationParameters = new TokenValidationParameters
            //        //{
            //        //    ValidTypes = new[] { "at+jwt" },
            //        //    ValidateAudience = false,
            //        //    NameClaimType = JwtClaimTypes.Name,
            //        //    RoleClaimType = JwtClaimTypes.Role
            //        //};
            //        options.SaveToken = true;

            //        if (Environment.IsDevelopment())
            //        {
            //            options.RequireHttpsMetadata = false;
            //        }
            //    }
            //);
            //if (Environment.IsDevelopment())
            //{
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            //}

            //services.AddLocalApiAuthentication();

            //services.AddAuthorization(options => {
            //    options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy => {
            //        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
            //        policy.RequireAuthenticatedUser();
            //    });
            //});
        }
    }
}
