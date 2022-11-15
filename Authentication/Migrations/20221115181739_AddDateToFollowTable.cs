using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication_temp.Migrations
{
    public partial class AddDateToFollowTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "followDate",
                table: "follows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followDate",
                table: "follows");
        }
    }
}
