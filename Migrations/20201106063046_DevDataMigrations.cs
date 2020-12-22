using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HyosungManagement.Migrations
{
    public partial class DevDataMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenu_MealPackage_PackageID",
                table: "DailyMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeRole_EmployeeRoleID",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_FoodCategory_CategoryID",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationLog_DailyMenu_DailyMenuID",
                table: "OperationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_GroupID",
                table: "Service");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BreakfastAssignment");

            migrationBuilder.DropTable(
                name: "DinnerAssignment");

            migrationBuilder.DropTable(
                name: "LunchAssignment");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SecurityCode");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmployeeRoleID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerServiceAssignment",
                table: "CustomerServiceAssignment");

            migrationBuilder.DropColumn(
                name: "ProteinAmount",
                table: "MealPackage");

            migrationBuilder.DropColumn(
                name: "MealCategory",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "EmployeeRoleID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MealCategory",
                table: "CustomerServiceAssignment");

            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "Customer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ServiceGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "ServiceGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "Service",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Service",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Service",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Service",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Service",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MealType",
                table: "OperationLog",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DailyMenuID",
                table: "OperationLog",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "OperationLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "MealPackage",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MealPackage",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "MealPackage",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "MealPackage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Meal",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Meal",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Meal",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "FoodIngredient",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "FoodIngredient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FoodIngredient",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "FoodIngredient",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FoodCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "FoodCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Food",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Food",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Food",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeRole",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "EmployeeRole",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "EmployeeRole",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LicenseRenewalDate",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Employee",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Employee",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Employee",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PackageID",
                table: "DailyMenu",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DailyMenu",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "DailyMenu",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerTag",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "CustomerTag",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "CustomerServiceAssignment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdmissionDate",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDischarged",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerServiceAssignment",
                table: "CustomerServiceAssignment",
                columns: new[] { "CustomerID", "ServiceID", "MealType" });

            migrationBuilder.CreateTable(
                name: "FoodIngredientCategory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodIngredientCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MealPackageAssignment",
                columns: table => new
                {
                    MealType = table.Column<string>(nullable: false),
                    MealID = table.Column<int>(nullable: false),
                    PackageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackageAssignment", x => new { x.MealID, x.MealType, x.PackageID });
                    table.ForeignKey(
                        name: "FK_MealPackageAssignment_Meal_MealID",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPackageAssignment_MealPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "MealPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreservationLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealType = table.Column<string>(nullable: false),
                    MealCategory = table.Column<string>(nullable: false),
                    DateIn = table.Column<DateTime>(nullable: false),
                    DateOut = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    AssignedMenuID = table.Column<int>(nullable: true),
                    ManagerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreservationLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreservationLog_DailyMenu_AssignedMenuID",
                        column: x => x.AssignedMenuID,
                        principalTable: "DailyMenu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreservationLog_EmployeeRole_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "EmployeeRole",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    ReportType = table.Column<string>(nullable: false),
                    LogDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    GeneratedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodIngredient_CategoryID",
                table: "FoodIngredient",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_RoleID",
                table: "Employee",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_MealPackageAssignment_PackageID",
                table: "MealPackageAssignment",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_PreservationLog_AssignedMenuID",
                table: "PreservationLog",
                column: "AssignedMenuID");

            migrationBuilder.CreateIndex(
                name: "IX_PreservationLog_ManagerID",
                table: "PreservationLog",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenu_MealPackage_PackageID",
                table: "DailyMenu",
                column: "PackageID",
                principalTable: "MealPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeRole_RoleID",
                table: "Employee",
                column: "RoleID",
                principalTable: "EmployeeRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_FoodCategory_CategoryID",
                table: "Food",
                column: "CategoryID",
                principalTable: "FoodCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodIngredient_FoodIngredientCategory_CategoryID",
                table: "FoodIngredient",
                column: "CategoryID",
                principalTable: "FoodIngredientCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationLog_DailyMenu_DailyMenuID",
                table: "OperationLog",
                column: "DailyMenuID",
                principalTable: "DailyMenu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceGroup_GroupID",
                table: "Service",
                column: "GroupID",
                principalTable: "ServiceGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenu_MealPackage_PackageID",
                table: "DailyMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeRole_RoleID",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_FoodCategory_CategoryID",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodIngredient_FoodIngredientCategory_CategoryID",
                table: "FoodIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationLog_DailyMenu_DailyMenuID",
                table: "OperationLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_GroupID",
                table: "Service");

            migrationBuilder.DropTable(
                name: "FoodIngredientCategory");

            migrationBuilder.DropTable(
                name: "MealPackageAssignment");

            migrationBuilder.DropTable(
                name: "PreservationLog");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropIndex(
                name: "IX_FoodIngredient_CategoryID",
                table: "FoodIngredient");

            migrationBuilder.DropIndex(
                name: "IX_Employee_RoleID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerServiceAssignment",
                table: "CustomerServiceAssignment");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "OperationLog");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MealPackage");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "MealPackage");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "MealPackage");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Meal");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "FoodIngredient");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FoodIngredient");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "FoodIngredient");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FoodCategory");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "FoodCategory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "EmployeeRole");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DailyMenu");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "DailyMenu");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerTag");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "CustomerTag");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "CustomerServiceAssignment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsDischarged",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "Service",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Service",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MealType",
                table: "OperationLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "DailyMenuID",
                table: "OperationLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "MealPackage",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProteinAmount",
                table: "MealPackage",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MealCategory",
                table: "Meal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "FoodIngredient",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Food",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LicenseRenewalDate",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeRoleID",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PackageID",
                table: "DailyMenu",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MealCategory",
                table: "CustomerServiceAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdmissionDate",
                table: "Customer",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerNumber",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerServiceAssignment",
                table: "CustomerServiceAssignment",
                columns: new[] { "CustomerID", "ServiceID", "MealCategory" });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreakfastAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    PackageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakfastAssignment", x => new { x.MealID, x.PackageID });
                    table.ForeignKey(
                        name: "FK_BreakfastAssignment_Meal_MealID",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreakfastAssignment_MealPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "MealPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DinnerAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    PackageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DinnerAssignment", x => new { x.MealID, x.PackageID });
                    table.ForeignKey(
                        name: "FK_DinnerAssignment_Meal_MealID",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DinnerAssignment_MealPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "MealPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LunchAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    PackageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunchAssignment", x => new { x.MealID, x.PackageID });
                    table.ForeignKey(
                        name: "FK_LunchAssignment_Meal_MealID",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LunchAssignment_MealPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "MealPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityCode",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeType = table.Column<int>(type: "int", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityCodeID = table.Column<int>(type: "int", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_SecurityCode_SecurityCodeID",
                        column: x => x.SecurityCodeID,
                        principalTable: "SecurityCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeRoleID",
                table: "Employee",
                column: "EmployeeRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SecurityCodeID",
                table: "AspNetUsers",
                column: "SecurityCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_BreakfastAssignment_PackageID",
                table: "BreakfastAssignment",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerAssignment_PackageID",
                table: "DinnerAssignment",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_LunchAssignment_PackageID",
                table: "LunchAssignment",
                column: "PackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenu_MealPackage_PackageID",
                table: "DailyMenu",
                column: "PackageID",
                principalTable: "MealPackage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeRole_EmployeeRoleID",
                table: "Employee",
                column: "EmployeeRoleID",
                principalTable: "EmployeeRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_FoodCategory_CategoryID",
                table: "Food",
                column: "CategoryID",
                principalTable: "FoodCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationLog_DailyMenu_DailyMenuID",
                table: "OperationLog",
                column: "DailyMenuID",
                principalTable: "DailyMenu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceGroup_GroupID",
                table: "Service",
                column: "GroupID",
                principalTable: "ServiceGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
