using Microsoft.EntityFrameworkCore.Migrations;

namespace HyosungManagement.Migrations
{
    public partial class Migrations5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "PostCategory",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowBoard",
                table: "PostCategory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "PostCategory");

            migrationBuilder.DropColumn(
                name: "ShowBoard",
                table: "PostCategory");
        }
    }
}
