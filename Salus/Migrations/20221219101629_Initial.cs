using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salus.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "auths",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passwordHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "longblob", nullable: false),
                    isAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    verificationToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    passwordResetToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resetTokenExpires = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auths", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "oils",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    kcalIn14Ml = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oils", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userProfiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    weight = table.Column<double>(type: "double", nullable: false),
                    height = table.Column<double>(type: "double", nullable: false),
                    birthDate = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<int>(type: "int", nullable: false),
                    goalWeight = table.Column<double>(type: "double", nullable: false),
                    hairIndex = table.Column<int>(type: "int", nullable: false),
                    skinIndex = table.Column<int>(type: "int", nullable: false),
                    eyesIndex = table.Column<int>(type: "int", nullable: false),
                    mouthIndex = table.Column<int>(type: "int", nullable: false),
                    authOfProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userProfiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_userProfiles_auths_authOfProfileId",
                        column: x => x.authOfProfileId,
                        principalTable: "auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fromId = table.Column<int>(type: "int", nullable: false),
                    toId = table.Column<int>(type: "int", nullable: false),
                    body = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sendDate = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "followings",
                columns: table => new
                {
                    followerId = table.Column<int>(type: "int", nullable: false),
                    followedId = table.Column<int>(type: "int", nullable: false),
                    followDate = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "last24Hs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    verifeid = table.Column<bool>(type: "tinyint(1)", nullable: false),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "foods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersLikeRecipes",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    recipeId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecipesIncludeIngredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_comments_fromId",
                table: "comments",
                column: "fromId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_toId",
                table: "comments",
                column: "toId");

            migrationBuilder.CreateIndex(
                name: "IX_followings_followerId",
                table: "followings",
                column: "followerId");

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
                name: "IX_userProfiles_authOfProfileId",
                table: "userProfiles",
                column: "authOfProfileId",
                unique: true);

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
                name: "comments");

            migrationBuilder.DropTable(
                name: "followings");

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

            migrationBuilder.DropTable(
                name: "userProfiles");

            migrationBuilder.DropTable(
                name: "auths");
        }
    }
}
