using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HyosungManagement.Migrations
{
    public partial class DataMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    AdmissionDate = table.Column<DateTime>(nullable: false),
                    DischargeDate = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTag",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FoodIngredient",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Origin = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodIngredient", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealCategory = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MealPackage",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    Calories = table.Column<double>(nullable: false),
                    ProteinAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackage", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SecurityCode",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeType = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    GeneratedAt = table.Column<DateTime>(nullable: false),
                    ExpiresAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGroup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "CustomerTagAssignment",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    CustomerTagID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTagAssignment", x => new { x.CustomerID, x.CustomerTagID });
                    table.ForeignKey(
                        name: "FK_CustomerTagAssignment_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerTagAssignment_CustomerTag_CustomerTagID",
                        column: x => x.CustomerTagID,
                        principalTable: "CustomerTag",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeRoleID = table.Column<int>(nullable: false),
                    LicensedDate = table.Column<DateTime>(nullable: false),
                    LicenseRenewalDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeRole_EmployeeRoleID",
                        column: x => x.EmployeeRoleID,
                        principalTable: "EmployeeRole",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Food_FoodCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "FoodCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreakfastAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(nullable: false),
                    PackageID = table.Column<int>(nullable: false)
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
                name: "DailyMenu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServedDate = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    PackageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DailyMenu_MealPackage_PackageID",
                        column: x => x.PackageID,
                        principalTable: "MealPackage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DinnerAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(nullable: false),
                    PackageID = table.Column<int>(nullable: false)
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
                    MealID = table.Column<int>(nullable: false),
                    PackageID = table.Column<int>(nullable: false)
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    SecurityCodeID = table.Column<int>(nullable: false)
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
                name: "Service",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    GroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Service_ServiceGroup_GroupID",
                        column: x => x.GroupID,
                        principalTable: "ServiceGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodIngredientAssignment",
                columns: table => new
                {
                    FoodID = table.Column<int>(nullable: false),
                    FoodIngredientID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodIngredientAssignment", x => new { x.FoodID, x.FoodIngredientID });
                    table.ForeignKey(
                        name: "FK_FoodIngredientAssignment_Food_FoodID",
                        column: x => x.FoodID,
                        principalTable: "Food",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodIngredientAssignment_FoodIngredient_FoodIngredientID",
                        column: x => x.FoodIngredientID,
                        principalTable: "FoodIngredient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealFoodAssignment",
                columns: table => new
                {
                    MealID = table.Column<int>(nullable: false),
                    FoodID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealFoodAssignment", x => new { x.MealID, x.FoodID });
                    table.ForeignKey(
                        name: "FK_MealFoodAssignment_Food_FoodID",
                        column: x => x.FoodID,
                        principalTable: "Food",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealFoodAssignment_Meal_MealID",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealType = table.Column<int>(nullable: false),
                    NumCustomersServed = table.Column<int>(nullable: false),
                    NumEmployeesServed = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DailyMenuID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OperationLog_DailyMenu_DailyMenuID",
                        column: x => x.DailyMenuID,
                        principalTable: "DailyMenu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CustomerServiceAssignment",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    MealCategory = table.Column<int>(nullable: false),
                    ServiceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerServiceAssignment", x => new { x.CustomerID, x.ServiceID, x.MealCategory });
                    table.ForeignKey(
                        name: "FK_CustomerServiceAssignment_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerServiceAssignment_Service_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Service",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CustomerServiceAssignment_ServiceID",
                table: "CustomerServiceAssignment",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTagAssignment_CustomerTagID",
                table: "CustomerTagAssignment",
                column: "CustomerTagID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenu_PackageID",
                table: "DailyMenu",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_DinnerAssignment_PackageID",
                table: "DinnerAssignment",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeRoleID",
                table: "Employee",
                column: "EmployeeRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Food_CategoryID",
                table: "Food",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_FoodIngredientAssignment_FoodIngredientID",
                table: "FoodIngredientAssignment",
                column: "FoodIngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_LunchAssignment_PackageID",
                table: "LunchAssignment",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_MealFoodAssignment_FoodID",
                table: "MealFoodAssignment",
                column: "FoodID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLog_DailyMenuID",
                table: "OperationLog",
                column: "DailyMenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Service_GroupID",
                table: "Service",
                column: "GroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CustomerServiceAssignment");

            migrationBuilder.DropTable(
                name: "CustomerTagAssignment");

            migrationBuilder.DropTable(
                name: "DinnerAssignment");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "FoodIngredientAssignment");

            migrationBuilder.DropTable(
                name: "LunchAssignment");

            migrationBuilder.DropTable(
                name: "MealFoodAssignment");

            migrationBuilder.DropTable(
                name: "OperationLog");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerTag");

            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropTable(
                name: "FoodIngredient");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "DailyMenu");

            migrationBuilder.DropTable(
                name: "SecurityCode");

            migrationBuilder.DropTable(
                name: "ServiceGroup");

            migrationBuilder.DropTable(
                name: "FoodCategory");

            migrationBuilder.DropTable(
                name: "MealPackage");
        }
    }
}
