using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus.Migrations
{
    public partial class Nullable24H : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_last24Hs_last24hid",
                table: "foods");

            migrationBuilder.AlterColumn<int>(
                name: "last24hid",
                table: "foods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_foods_last24Hs_last24hid",
                table: "foods",
                column: "last24hid",
                principalTable: "last24Hs",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foods_last24Hs_last24hid",
                table: "foods");

            migrationBuilder.AlterColumn<int>(
                name: "last24hid",
                table: "foods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_foods_last24Hs_last24hid",
                table: "foods",
                column: "last24hid",
                principalTable: "last24Hs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
