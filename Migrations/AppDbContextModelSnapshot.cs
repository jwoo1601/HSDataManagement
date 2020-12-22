﻿// <auto-generated />
using System;
using HyosungManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HyosungManagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HyosungManagement.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AdmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("DischargeDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDischarged")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("HyosungManagement.Models.CustomerServiceAssignment", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<string>("MealType")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CustomerID", "ServiceID", "MealType");

                    b.HasIndex("ServiceID");

                    b.ToTable("CustomerServiceAssignment");
                });

            modelBuilder.Entity("HyosungManagement.Models.CustomerTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("CustomerTag");
                });

            modelBuilder.Entity("HyosungManagement.Models.CustomerTagAssignment", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("CustomerTagID")
                        .HasColumnType("int");

                    b.HasKey("CustomerID", "CustomerTagID");

                    b.HasIndex("CustomerTagID");

                    b.ToTable("CustomerTagAssignment");
                });

            modelBuilder.Entity("HyosungManagement.Models.DailyMenu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int?>("PackageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ServedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("PackageID");

                    b.ToTable("DailyMenu");
                });

            modelBuilder.Entity("HyosungManagement.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("LicenseRenewalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LicensedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int?>("RoleID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("HyosungManagement.Models.EmployeeRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.ToTable("EmployeeRole");
                });

            modelBuilder.Entity("HyosungManagement.Models.Food", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("FoodCategory");
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodIngredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("FoodIngredient");
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodIngredientAssignment", b =>
                {
                    b.Property<int>("FoodID")
                        .HasColumnType("int");

                    b.Property<int>("FoodIngredientID")
                        .HasColumnType("int");

                    b.HasKey("FoodID", "FoodIngredientID");

                    b.HasIndex("FoodIngredientID");

                    b.ToTable("FoodIngredientAssignment");
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodIngredientCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("FoodIngredientCategory");
                });

            modelBuilder.Entity("HyosungManagement.Models.Meal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("HyosungManagement.Models.MealFoodAssignment", b =>
                {
                    b.Property<int>("MealID")
                        .HasColumnType("int");

                    b.Property<int>("FoodID")
                        .HasColumnType("int");

                    b.HasKey("MealID", "FoodID");

                    b.HasIndex("FoodID");

                    b.ToTable("MealFoodAssignment");
                });

            modelBuilder.Entity("HyosungManagement.Models.MealPackage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Calories")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<double?>("Protein")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.ToTable("MealPackage");
                });

            modelBuilder.Entity("HyosungManagement.Models.MealPackageAssignment", b =>
                {
                    b.Property<int>("MealID")
                        .HasColumnType("int");

                    b.Property<string>("MealType")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PackageID")
                        .HasColumnType("int");

                    b.HasKey("MealID", "MealType", "PackageID");

                    b.HasIndex("PackageID");

                    b.ToTable("MealPackageAssignment");
                });

            modelBuilder.Entity("HyosungManagement.Models.OperationLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("DailyMenuID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("MealType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumCustomersServed")
                        .HasColumnType("int");

                    b.Property<int>("NumEmployeesServed")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DailyMenuID");

                    b.ToTable("OperationLog");
                });

            modelBuilder.Entity("HyosungManagement.Models.PreservationLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssignedMenuID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("ManagerID")
                        .HasColumnType("int");

                    b.Property<string>("MealCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MealType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AssignedMenuID");

                    b.HasIndex("ManagerID");

                    b.ToTable("PreservationLog");
                });

            modelBuilder.Entity("HyosungManagement.Models.Report", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("GeneratedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReportType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("HyosungManagement.Models.Service", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<int?>("GroupID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.HasIndex("GroupID");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("HyosungManagement.Models.ServiceGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("ServiceGroup");
                });

            modelBuilder.Entity("HyosungManagement.Models.CustomerServiceAssignment", b =>
                {
                    b.HasOne("HyosungManagement.Models.Customer", "Customer")
                        .WithMany("ServiceAssignments")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyosungManagement.Models.Service", "Service")
                        .WithMany("CustomerAssignments")
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyosungManagement.Models.CustomerTagAssignment", b =>
                {
                    b.HasOne("HyosungManagement.Models.Customer", "Customer")
                        .WithMany("TagAssignments")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyosungManagement.Models.CustomerTag", "Tag")
                        .WithMany("CustomerAssignments")
                        .HasForeignKey("CustomerTagID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyosungManagement.Models.DailyMenu", b =>
                {
                    b.HasOne("HyosungManagement.Models.MealPackage", "Package")
                        .WithMany("AssignedMenus")
                        .HasForeignKey("PackageID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("HyosungManagement.Models.Employee", b =>
                {
                    b.HasOne("HyosungManagement.Models.EmployeeRole", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("HyosungManagement.Models.Food", b =>
                {
                    b.HasOne("HyosungManagement.Models.FoodCategory", "Category")
                        .WithMany("Foods")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodIngredient", b =>
                {
                    b.HasOne("HyosungManagement.Models.FoodIngredientCategory", "Category")
                        .WithMany("Ingredients")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("HyosungManagement.Models.FoodIngredientAssignment", b =>
                {
                    b.HasOne("HyosungManagement.Models.Food", "Food")
                        .WithMany("IngredientAssignments")
                        .HasForeignKey("FoodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyosungManagement.Models.FoodIngredient", "Ingredient")
                        .WithMany("FoodAssignments")
                        .HasForeignKey("FoodIngredientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyosungManagement.Models.MealFoodAssignment", b =>
                {
                    b.HasOne("HyosungManagement.Models.Food", "Food")
                        .WithMany("MealAssignments")
                        .HasForeignKey("FoodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyosungManagement.Models.Meal", "Meal")
                        .WithMany("FoodAssignments")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyosungManagement.Models.MealPackageAssignment", b =>
                {
                    b.HasOne("HyosungManagement.Models.Meal", "Meal")
                        .WithMany("PackageAssignments")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyosungManagement.Models.MealPackage", "Package")
                        .WithMany("MealAssignments")
                        .HasForeignKey("PackageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyosungManagement.Models.OperationLog", b =>
                {
                    b.HasOne("HyosungManagement.Models.DailyMenu", "DailyMenu")
                        .WithMany("OperationLogs")
                        .HasForeignKey("DailyMenuID");
                });

            modelBuilder.Entity("HyosungManagement.Models.PreservationLog", b =>
                {
                    b.HasOne("HyosungManagement.Models.DailyMenu", "AssignedMenu")
                        .WithMany("PreservationLogs")
                        .HasForeignKey("AssignedMenuID");

                    b.HasOne("HyosungManagement.Models.EmployeeRole", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerID");
                });

            modelBuilder.Entity("HyosungManagement.Models.Service", b =>
                {
                    b.HasOne("HyosungManagement.Models.ServiceGroup", "Group")
                        .WithMany("Services")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
