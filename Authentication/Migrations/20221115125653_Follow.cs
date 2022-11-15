using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class Follow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    followingId = table.Column<int>(type: "int", nullable: false),
                    followerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follows", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUserProfile");

            migrationBuilder.DropTable(
                name: "follows");
        }
    }
}
