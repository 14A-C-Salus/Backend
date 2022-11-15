using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class ConnectUserProfileToFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUserProfile");

            migrationBuilder.CreateTable(
                name: "followUserProfiles",
                columns: table => new
                {
                    followId = table.Column<int>(type: "int", nullable: false),
                    userProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followUserProfiles", x => new { x.followId, x.userProfileId });
                    table.ForeignKey(
                        name: "FK_followUserProfiles_follows_followId",
                        column: x => x.followId,
                        principalTable: "follows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_followUserProfiles_userProfiles_userProfileId",
                        column: x => x.userProfileId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_followUserProfiles_userProfileId",
                table: "followUserProfiles",
                column: "userProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followUserProfiles");

            migrationBuilder.CreateTable(
                name: "FollowUserProfile",
                columns: table => new
                {
                    followsid = table.Column<int>(type: "int", nullable: false),
                    userProfilesid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUserProfile", x => new { x.followsid, x.userProfilesid });
                    table.ForeignKey(
                        name: "FK_FollowUserProfile_follows_followsid",
                        column: x => x.followsid,
                        principalTable: "follows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUserProfile_userProfiles_userProfilesid",
                        column: x => x.userProfilesid,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUserProfile_userProfilesid",
                table: "FollowUserProfile",
                column: "userProfilesid");
        }
    }
}
