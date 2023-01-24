using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus.Migrations
{
    public partial class Update_ManytoMany_rss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodsHaveTags_foods_tagId",
                table: "FoodsHaveTags");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodsHaveTags_tags_foodId",
                table: "FoodsHaveTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikeRecipes_recipes_userId",
                table: "UsersLikeRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikeRecipes_userProfiles_recipeId",
                table: "UsersLikeRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferTags_tags_userId",
                table: "UsersPreferTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferTags_userProfiles_tagId",
                table: "UsersPreferTags");

            migrationBuilder.AlterColumn<int>(
                name: "min",
                table: "tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "max",
                table: "tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "foodProperty",
                table: "tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodsHaveTags_foods_foodId",
                table: "FoodsHaveTags",
                column: "foodId",
                principalTable: "foods",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodsHaveTags_tags_tagId",
                table: "FoodsHaveTags",
                column: "tagId",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikeRecipes_recipes_recipeId",
                table: "UsersLikeRecipes",
                column: "recipeId",
                principalTable: "recipes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikeRecipes_userProfiles_userId",
                table: "UsersLikeRecipes",
                column: "userId",
                principalTable: "userProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferTags_tags_tagId",
                table: "UsersPreferTags",
                column: "tagId",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferTags_userProfiles_userId",
                table: "UsersPreferTags",
                column: "userId",
                principalTable: "userProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodsHaveTags_foods_foodId",
                table: "FoodsHaveTags");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodsHaveTags_tags_tagId",
                table: "FoodsHaveTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikeRecipes_recipes_recipeId",
                table: "UsersLikeRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersLikeRecipes_userProfiles_userId",
                table: "UsersLikeRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferTags_tags_tagId",
                table: "UsersPreferTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersPreferTags_userProfiles_userId",
                table: "UsersPreferTags");

            migrationBuilder.AlterColumn<int>(
                name: "min",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "max",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "foodProperty",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodsHaveTags_foods_tagId",
                table: "FoodsHaveTags",
                column: "tagId",
                principalTable: "foods",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodsHaveTags_tags_foodId",
                table: "FoodsHaveTags",
                column: "foodId",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikeRecipes_recipes_userId",
                table: "UsersLikeRecipes",
                column: "userId",
                principalTable: "recipes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersLikeRecipes_userProfiles_recipeId",
                table: "UsersLikeRecipes",
                column: "recipeId",
                principalTable: "userProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferTags_tags_userId",
                table: "UsersPreferTags",
                column: "userId",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPreferTags_userProfiles_tagId",
                table: "UsersPreferTags",
                column: "tagId",
                principalTable: "userProfiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
