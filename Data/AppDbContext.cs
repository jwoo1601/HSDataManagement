using HyosungManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerTag> CustomerTags { get; set; }
        public DbSet<CustomerTagAssignment> CustomerTagAssignments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<CustomerServiceAssignment> CustomerServiceAssignments { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<FoodIngredient> FoodIngredients { get; set; }
        public DbSet<FoodIngredientCategory> FoodIngredientCategories { get; set; }
        public DbSet<FoodIngredientAssignment> FoodIngredientAssignments { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealFoodAssignment> MealFoodAssignments { get; set; }
        public DbSet<MealPackage> MealPackages { get; set; }
        public DbSet<MealPackageAssignment> MealPackageAssignments { get; set; }
        //public DbSet<BreakfastAssignment> BreakfastAssignments { get; set; }
        //public DbSet<LunchAssignment> LunchAssignments { get; set; }
        //public DbSet<DinnerAssignment> DinnerAssignments { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<PreservationLog> PreservationLogs { get; set; }
        public DbSet<DailyMenu> DailyMenus { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Report> Reports { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(e => {
                e.ToTable("Customer")
                    .HasQueryFilter(c => !c.IsDeleted);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<CustomerTag>(e => {
                e.ToTable("CustomerTag");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<CustomerTagAssignment>().ToTable("CustomerTagAssignment");
            modelBuilder.Entity<Service>(e => {
                e.ToTable("Service")
                    .HasOne(s => s.Group)
                    .WithMany(sg => sg.Services)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<ServiceGroup>(e => {
                e.ToTable("ServiceGroup");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<CustomerServiceAssignment>(e => {
                e.ToTable("CustomerServiceAssignment")
                    .Property(a => a.MealType)
                    .HasConversion(new EnumToStringConverter<MealType>());
            });
            modelBuilder.Entity<Food>(e => {
                e.ToTable("Food")
                    .HasOne(f => f.Category)
                    .WithMany(fc => fc.Foods)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<FoodCategory>(e => {
                e.ToTable("FoodCategory");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<FoodIngredient>(e => {
                e.ToTable("FoodIngredient")
                    .HasOne(fi => fi.Category)
                    .WithMany(fic => fic.Ingredients)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<FoodIngredientCategory>(e => {
                e.ToTable("FoodIngredientCategory");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<FoodIngredientAssignment>().ToTable("FoodIngredientAssignment");
            modelBuilder.Entity<Meal>(e => {
                e.ToTable("Meal")
                    .Property(m => m.Category)
                    .HasConversion(new EnumToStringConverter<MealCategory>());

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<MealFoodAssignment>().ToTable("MealFoodAssignment");
            modelBuilder.Entity<MealPackage>(e => {
                e.ToTable("MealPackage");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<MealPackageAssignment>(e => {
                e.ToTable("MealPackageAssignment");
                //.HasDiscriminator(a => a.MealType)
                //.HasValue<BreakfastAssignment>("breakfast")
                //.HasValue<LunchAssignment>("lunch")
                //.HasValue<DinnerAssignment>("dinner");

                //e.Property(a => a.MealType)
                //    .HasMaxLength(50);

                e.Property(a => a.MealType)
                    .HasConversion(new EnumToStringConverter<MealType>());
            });
            //modelBuilder.Entity<BreakfastAssignment>().ToTable("BreakfastAssignment");
            //modelBuilder.Entity<LunchAssignment>().ToTable("LunchAssignment");
            //modelBuilder.Entity<DinnerAssignment>().ToTable("DinnerAssignment");
            modelBuilder.Entity<OperationLog>(e => {
                e.ToTable("OperationLog")
                    .Property(op => op.MealType)
                    .HasConversion(new EnumToStringConverter<MealType>());

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<PreservationLog>(e => {
                e.ToTable("PreservationLog");

                e.Property(pr => pr.MealType)
                    .HasConversion(new EnumToStringConverter<MealType>());

                e.Property(pr => pr.MealCategory)
                    .HasConversion(new EnumToStringConverter<MealCategory>());

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<DailyMenu>(e => {
                e.ToTable("DailyMenu")
                    .HasOne(dm => dm.Package)
                    .WithMany(p => p.AssignedMenus)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<Employee>(e => {
                e.ToTable("Employee")
                    .HasOne(e => e.Role)
                    .WithMany(er => er.Employees)
                    .OnDelete(DeleteBehavior.SetNull);

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<EmployeeRole>(e => {
                e.ToTable("EmployeeRole");

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
            modelBuilder.Entity<Report>(e => {
                e.ToTable("Report");

                e.Property(r => r.ReportType)
                    .HasConversion(new EnumToStringConverter<ReportType>());

                e.Property(entity => entity.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                e.Property(entity => entity.LastUpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            #region Customer - CustomerTag Many-to-Many
            modelBuilder.Entity<CustomerTagAssignment>()
                        .HasKey(a => new { a.CustomerID, a.CustomerTagID });
            modelBuilder.Entity<CustomerTagAssignment>()
                        .HasOne(a => a.Customer)
                        .WithMany(c => c.TagAssignments)
                        .HasForeignKey(a => a.CustomerID);
            modelBuilder.Entity<CustomerTagAssignment>()
                        .HasOne(a => a.Tag)
                        .WithMany(c => c.CustomerAssignments)
                        .HasForeignKey(a => a.CustomerTagID);
            #endregion

            #region Customer - Service Many-to-Many
            modelBuilder.Entity<CustomerServiceAssignment>()
                        .HasKey(a => new { a.CustomerID, a.ServiceID, a.MealType });
            modelBuilder.Entity<CustomerServiceAssignment>()
                        .HasOne(a => a.Customer)
                        .WithMany(c => c.ServiceAssignments)
                        .HasForeignKey(a => a.CustomerID);
            modelBuilder.Entity<CustomerServiceAssignment>()
                        .HasOne(a => a.Service)
                        .WithMany(c => c.CustomerAssignments)
                        .HasForeignKey(a => a.ServiceID);
            #endregion

            #region Food - FoodIngredient Many-to-Many
            modelBuilder.Entity<FoodIngredientAssignment>()
                        .HasKey(a => new { a.FoodID, a.FoodIngredientID });
            modelBuilder.Entity<FoodIngredientAssignment>()
                        .HasOne(a => a.Food)
                        .WithMany(c => c.IngredientAssignments)
                        .HasForeignKey(a => a.FoodID);
            modelBuilder.Entity<FoodIngredientAssignment>()
                        .HasOne(a => a.Ingredient)
                        .WithMany(c => c.FoodAssignments)
                        .HasForeignKey(a => a.FoodIngredientID);
            #endregion

            #region Meal - Food Many-to-Many
            modelBuilder.Entity<MealFoodAssignment>()
                        .HasKey(a => new { a.MealID, a.FoodID });
            modelBuilder.Entity<MealFoodAssignment>()
                        .HasOne(a => a.Meal)
                        .WithMany(c => c.FoodAssignments)
                        .HasForeignKey(a => a.MealID);
            modelBuilder.Entity<MealFoodAssignment>()
                        .HasOne(a => a.Food)
                        .WithMany(c => c.MealAssignments)
                        .HasForeignKey(a => a.FoodID);
            #endregion

            #region Meal - MealPackage Many-to-Many
            modelBuilder.Entity<MealPackageAssignment>()
                        .HasKey(a => new { a.MealID, a.MealType, a.PackageID });
            modelBuilder.Entity<MealPackageAssignment>()
                        .HasOne(a => a.Meal)
                        .WithMany(c => c.PackageAssignments)
                        .HasForeignKey(a => a.MealID);
            modelBuilder.Entity<MealPackageAssignment>()
                        .HasOne(a => a.Package)
                        .WithMany(c => c.MealAssignments)
                        .HasForeignKey(a => a.PackageID);
            #endregion

            //#region Meal - BreakfastPackage Many-to-Many
            //modelBuilder.Entity<BreakfastAssignment>()
            //            .HasKey(a => new { a.MealID, a.PackageID });
            //modelBuilder.Entity<BreakfastAssignment>()
            //            .HasOne(a => a.Meal)
            //            .WithMany(c => c.BreakfastPackageAssignments)
            //            .HasForeignKey(a => a.MealID);
            //modelBuilder.Entity<BreakfastAssignment>()
            //            .HasOne(a => a.Package)
            //            .WithMany(c => c.BreakfastMealAssignments)
            //            .HasForeignKey(a => a.PackageID);
            //#endregion

            //#region Meal - LunchPackage Many-to-Many
            //modelBuilder.Entity<LunchAssignment>()
            //            .HasKey(a => new { a.MealID, a.PackageID });
            //modelBuilder.Entity<LunchAssignment>()
            //            .HasOne(a => a.Meal)
            //            .WithMany(c => c.LunchPackageAssignments)
            //            .HasForeignKey(a => a.MealID);
            //modelBuilder.Entity<LunchAssignment>()
            //            .HasOne(a => a.Package)
            //            .WithMany(c => c.LunchMealAssignments)
            //            .HasForeignKey(a => a.PackageID);
            //#endregion

            //#region Meal - DinnerPackage Many-to-Many
            //modelBuilder.Entity<DinnerAssignment>()
            //            .HasKey(a => new { a.MealID, a.PackageID });
            //modelBuilder.Entity<DinnerAssignment>()
            //            .HasOne(a => a.Meal)
            //            .WithMany(c => c.DinnerPackageAssignments)
            //            .HasForeignKey(a => a.MealID);
            //modelBuilder.Entity<DinnerAssignment>()
            //            .HasOne(a => a.Package)
            //            .WithMany(c => c.DinnerMealAssignments)
            //            .HasForeignKey(a => a.PackageID);
            //#endregion
        }
    }
}
