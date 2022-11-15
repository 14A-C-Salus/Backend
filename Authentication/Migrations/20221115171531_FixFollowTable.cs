using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class FixFollowTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followerId",
                table: "follows");

            migrationBuilder.RenameColumn(
                name: "followingId",
                table: "follows",
                newName: "followedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "followedId",
                table: "follows",
                newName: "followingId");

            migrationBuilder.AddColumn<int>(
                name: "followerId",
                table: "follows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
