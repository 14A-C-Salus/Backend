using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus.Migrations
{
    public partial class Fix_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_userProfiles_Authorid",
                table: "recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_recipes_Recipeid",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_tags_Recipeid",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_recipes_Authorid",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "Recipeid",
                table: "tags");

            migrationBuilder.RenameColumn(
                name: "Authorid",
                table: "recipes",
                newName: "timeInMinute");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "recipes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "gramm",
                table: "recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "method",
                table: "recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "oilPortionMl",
                table: "recipes",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "gramm",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "method",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "oilPortionMl",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "timeInMinute",
                table: "recipes",
                newName: "Authorid");

            migrationBuilder.AddColumn<int>(
                name: "Recipeid",
                table: "tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_Recipeid",
                table: "tags",
                column: "Recipeid");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_Authorid",
                table: "recipes",
                column: "Authorid");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_userProfiles_Authorid",
                table: "recipes",
                column: "Authorid",
                principalTable: "userProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tags_recipes_Recipeid",
                table: "tags",
                column: "Recipeid",
                principalTable: "recipes",
                principalColumn: "id");
        }
    }
}
