using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class Followers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userProfileToUserProfile",
                columns: table => new
                {
                    followerId = table.Column<int>(type: "int", nullable: false),
                    followedId = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false),
                    followDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userProfileToUserProfile", x => new { x.followedId, x.followerId });
                    table.ForeignKey(
                        name: "FK_userProfileToUserProfile_userProfiles_followedId",
                        column: x => x.followedId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userProfileToUserProfile_userProfiles_followerId",
                        column: x => x.followerId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userProfileToUserProfile_followerId",
                table: "userProfileToUserProfile",
                column: "followerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userProfileToUserProfile");
        }
    }
}
