using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus_temp.Migrations
{
    public partial class RenameJoiningEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userProfileToUserProfile");

            migrationBuilder.CreateTable(
                name: "followings",
                columns: table => new
                {
                    followerId = table.Column<int>(type: "int", nullable: false),
                    followedId = table.Column<int>(type: "int", nullable: false),
                    followDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followings", x => new { x.followedId, x.followerId });
                    table.ForeignKey(
                        name: "FK_followings_userProfiles_followedId",
                        column: x => x.followedId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_followings_userProfiles_followerId",
                        column: x => x.followerId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_followings_followerId",
                table: "followings",
                column: "followerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followings");

            migrationBuilder.CreateTable(
                name: "userProfileToUserProfile",
                columns: table => new
                {
                    followedId = table.Column<int>(type: "int", nullable: false),
                    followerId = table.Column<int>(type: "int", nullable: false),
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
    }
}
