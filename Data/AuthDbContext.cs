using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HyosungManagement.Data
{
    public class AuthDbContext :
        DbContext,
        IConfigurationDbContext,
        IPersistedGrantDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        private readonly IConfiguration configuration;

        public AuthDbContext(
            DbContextOptions<AuthDbContext> options,
            IConfiguration configuration
        )
            : base(options)
        {
            this.configuration = configuration;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder
                .UseSqlServer(
                    configuration.GetConnectionString("Auth")
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<ClientCorsOrigin>().ToTable("ClientCorsOrigin");
            modelBuilder.Entity<IdentityResource>().ToTable("IdentityResource");
            modelBuilder.Entity<ApiResource>().ToTable("ApiResource");
            modelBuilder.Entity<ApiScope>().ToTable("ApiScope");
            modelBuilder.Entity<PersistedGrant>(e => {
                e.ToTable("PersistedGrant")
                    .HasKey(pg => pg.Key);
            });
            modelBuilder.Entity<DeviceFlowCodes>(e => {
                e.ToTable("DeviceFlowCode")
                    .HasNoKey();
            });
        }
    }
}
