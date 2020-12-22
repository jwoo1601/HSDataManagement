using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HyosungManagement.Migrations.UserDb
{
    public partial class DataMigrations4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "User");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "User",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "User",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "User");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedTime",
                table: "User",
                type: "datetime2",
                nullable: true);
        }
    }
}
