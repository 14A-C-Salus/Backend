using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus_temp.Migrations
{
    public partial class Comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fromId = table.Column<int>(type: "int", nullable: false),
                    toId = table.Column<int>(type: "int", nullable: false),
                    body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    sendDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_userProfiles_fromId",
                        column: x => x.fromId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comments_userProfiles_toId",
                        column: x => x.toId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_fromId",
                table: "comments",
                column: "fromId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_toId",
                table: "comments",
                column: "toId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");
        }
    }
}
