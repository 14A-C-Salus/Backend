using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus_temp.Migrations
{
    public partial class CreateAllTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "last24Hs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    liquidInDl = table.Column<int>(type: "int", nullable: false),
                    minLiquidInDl = table.Column<int>(type: "int", nullable: false),
                    maxKcal = table.Column<int>(type: "int", nullable: false),
                    userProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_last24Hs", x => x.id);
                    table.ForeignKey(
                        name: "FK_last24Hs_userProfiles_userProfileId",
                        column: x => x.userProfileId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oils",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kcalIn14Ml = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oils", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    last24hid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foods", x => x.id);
                    table.ForeignKey(
                        name: "FK_foods_last24Hs_last24hid",
                        column: x => x.last24hid,
                        principalTable: "last24Hs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verifeid = table.Column<bool>(type: "bit", nullable: false),
                    Authorid = table.Column<int>(type: "int", nullable: false),
                    oilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipes_oils_oilId",
                        column: x => x.oilId,
                        principalTable: "oils",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_recipes_userProfiles_Authorid",
                        column: x => x.Authorid,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipesIncludeIngredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    portionInGramm = table.Column<int>(type: "int", nullable: false),
                    recipeId = table.Column<int>(type: "int", nullable: false),
                    foodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipesIncludeIngredients", x => x.id);
                    table.ForeignKey(
                        name: "FK_RecipesIncludeIngredients_foods_foodId",
                        column: x => x.foodId,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipesIncludeIngredients_recipes_recipeId",
                        column: x => x.recipeId,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_tags_recipes_Recipeid",
                        column: x => x.Recipeid,
                        principalTable: "recipes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UsersLikeRecipes",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    recipeId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersLikeRecipes", x => new { x.userId, x.recipeId });
                    table.ForeignKey(
                        name: "FK_UsersLikeRecipes_recipes_userId",
                        column: x => x.userId,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersLikeRecipes_userProfiles_recipeId",
                        column: x => x.recipeId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FoodsHaveTags",
                columns: table => new
                {
                    foodId = table.Column<int>(type: "int", nullable: false),
                    tagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodsHaveTags", x => new { x.foodId, x.tagId });
                    table.ForeignKey(
                        name: "FK_FoodsHaveTags_foods_tagId",
                        column: x => x.tagId,
                        principalTable: "foods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoodsHaveTags_tags_foodId",
                        column: x => x.foodId,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersPreferTags",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    tagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersPreferTags", x => new { x.userId, x.tagId });
                    table.ForeignKey(
                        name: "FK_UsersPreferTags_tags_userId",
                        column: x => x.userId,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersPreferTags_userProfiles_tagId",
                        column: x => x.tagId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_foods_last24hid",
                table: "foods",
                column: "last24hid");

            migrationBuilder.CreateIndex(
                name: "IX_FoodsHaveTags_tagId",
                table: "FoodsHaveTags",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_last24Hs_userProfileId",
                table: "last24Hs",
                column: "userProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipes_Authorid",
                table: "recipes",
                column: "Authorid");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_oilId",
                table: "recipes",
                column: "oilId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIncludeIngredients_foodId",
                table: "RecipesIncludeIngredients",
                column: "foodId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIncludeIngredients_recipeId",
                table: "RecipesIncludeIngredients",
                column: "recipeId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_Recipeid",
                table: "tags",
                column: "Recipeid");

            migrationBuilder.CreateIndex(
                name: "IX_UsersLikeRecipes_recipeId",
                table: "UsersLikeRecipes",
                column: "recipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPreferTags_tagId",
                table: "UsersPreferTags",
                column: "tagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodsHaveTags");

            migrationBuilder.DropTable(
                name: "RecipesIncludeIngredients");

            migrationBuilder.DropTable(
                name: "UsersLikeRecipes");

            migrationBuilder.DropTable(
                name: "UsersPreferTags");

            migrationBuilder.DropTable(
                name: "foods");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "last24Hs");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "oils");
        }
    }
}
