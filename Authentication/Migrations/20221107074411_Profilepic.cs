using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class Profilepic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "eyesIndex",
                table: "userProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hairIndex",
                table: "userProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mouthIndex",
                table: "userProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skinIndex",
                table: "userProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eyesIndex",
                table: "userProfiles");

            migrationBuilder.DropColumn(
                name: "hairIndex",
                table: "userProfiles");

            migrationBuilder.DropColumn(
                name: "mouthIndex",
                table: "userProfiles");

            migrationBuilder.DropColumn(
                name: "skinIndex",
                table: "userProfiles");
        }
    }
}
