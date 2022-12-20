using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus.Migrations
{
    public partial class OrganisationAndRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verifeid",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "kcalIn14Ml",
                table: "oils",
                newName: "calIn14Ml");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "tags",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "oils",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "verifeid",
                table: "foods",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "name",
                table: "oils");

            migrationBuilder.DropColumn(
                name: "verifeid",
                table: "foods");

            migrationBuilder.RenameColumn(
                name: "calIn14Ml",
                table: "oils",
                newName: "kcalIn14Ml");

            migrationBuilder.AddColumn<bool>(
                name: "verifeid",
                table: "recipes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
