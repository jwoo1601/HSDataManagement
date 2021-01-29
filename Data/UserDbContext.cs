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
    public class UserDbContext : IdentityDbContext<
        HSMUser,
        HSMRole,
        string,
        HSMUserClaim,
        HSMUserRole,
        HSMUserLogin,
        HSMRoleClaim,
        HSMUserToken
    >
    {
        public DbSet<SecurityCode> SecurityCodes { get; set; }

        private readonly IConfiguration configuration;

        public UserDbContext(
            DbContextOptions<UserDbContext> options,
            IConfiguration configuration
        )
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder
                .UseSqlServer(
                    configuration.GetConnectionString("User")
                )
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HSMUser>(entity => {
                entity.ToTable("User");

                entity.HasOne(e => e.SecurityCode)
                        .WithMany(sc => sc.Users)
                        .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(u => u.Roles)
                        .WithOne(ur => ur.User)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();

                entity.HasMany(u => u.Claims)
                        .WithOne(uc => uc.User)
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();

                entity.HasMany(u => u.Logins)
                        .WithOne(ul => ul.User)
                        .HasForeignKey(ul => ul.UserId)
                        .IsRequired();

                entity.HasMany(u => u.Tokens)
                        .WithOne(ut => ut.User)
                        .HasForeignKey(ut => ut.UserId)
                        .IsRequired();

                entity.Property(e => e.RegisteredAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<HSMRole>(entity => {
                entity.ToTable("Role");

                entity.HasMany(r => r.Users)
                        .WithOne(ur => ur.Role)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                entity.HasMany(r => r.Claims)
                        .WithOne(rc => rc.Role)
                        .HasForeignKey(rc => rc.RoleId)
                        .IsRequired();
            });

            modelBuilder.Entity<HSMRoleClaim>(entity => {
                entity.ToTable("RoleClaim");
            });

            modelBuilder.Entity<HSMUserRole>(entity => {
                entity.ToTable("UserRole");
            });

            modelBuilder.Entity<HSMUserClaim>(entity => {
                entity.ToTable("UserClaim");
            });

            modelBuilder.Entity<HSMUserLogin>(entity => {
                entity.ToTable("UserLogin");
            });

            modelBuilder.Entity<HSMUserToken>(entity => {
                entity.ToTable("UserToken");
            });

            modelBuilder.Entity<SecurityCode>(e => {
                e.ToTable("SecurityCode")
                    .Property(sc => sc.CodeType)
                    .HasConversion(new EnumToStringConverter<SecurityCodeType>());

                e.Property(e => e.GeneratedAt)
                 .HasDefaultValueSql("GETUTCDATE()")
                 .ValueGeneratedOnAdd();
            });
        }
    }
}
