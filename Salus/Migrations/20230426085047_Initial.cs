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
                name: "diets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    minFat = table.Column<int>(type: "int", nullable: true),
                    minCarbohydrate = table.Column<int>(type: "int", nullable: true),
                    minProtein = table.Column<int>(type: "int", nullable: true),
                    minKcal = table.Column<int>(type: "int", nullable: true),
                    minDl = table.Column<int>(type: "int", nullable: true),
                    maxFat = table.Column<int>(type: "int", nullable: true),
                    maxCarbohydrate = table.Column<int>(type: "int", nullable: true),
                    maxProtein = table.Column<int>(type: "int", nullable: true),
                    maxKcal = table.Column<int>(type: "int", nullable: true),
                    maxDl = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diets", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "oils",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    calIn14Ml = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oils", x => x.id);
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
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    recipeProperty = table.Column<int>(type: "int", nullable: true),
                    min = table.Column<int>(type: "int", nullable: true),
                    max = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
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
                    birthDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    goalWeight = table.Column<double>(type: "double", nullable: false),
                    hairIndex = table.Column<int>(type: "int", nullable: false),
                    skinIndex = table.Column<int>(type: "int", nullable: false),
                    eyesIndex = table.Column<int>(type: "int", nullable: false),
                    mouthIndex = table.Column<int>(type: "int", nullable: false),
                    dietid = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_userProfiles_diets_dietid",
                        column: x => x.dietid,
                        principalTable: "diets",
                        principalColumn: "id");
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
                    gramm = table.Column<int>(type: "int", nullable: false),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    liquidInDl = table.Column<int>(type: "int", nullable: true),
                    time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
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
                        name: "FK_UsersPreferTags_tags_tagId",
                        column: x => x.tagId,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersPreferTags_userProfiles_userId",
                        column: x => x.userId,
                        principalTable: "userProfiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    gramm = table.Column<int>(type: "int", nullable: false),
                    kcal = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<int>(type: "int", nullable: false),
                    fat = table.Column<int>(type: "int", nullable: false),
                    carbohydrate = table.Column<int>(type: "int", nullable: false),
                    verifeid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    timeInMinute = table.Column<int>(type: "int", nullable: false),
                    oilPortionMl = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    method = table.Column<int>(type: "int", nullable: false),
                    userProfileid = table.Column<int>(type: "int", nullable: true),
                    oilId = table.Column<int>(type: "int", nullable: true),
                    last24hid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipes_last24Hs_last24hid",
                        column: x => x.last24hid,
                        principalTable: "last24Hs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_recipes_oils_oilId",
                        column: x => x.oilId,
                        principalTable: "oils",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_recipes_userProfiles_userProfileid",
                        column: x => x.userProfileid,
                        principalTable: "userProfiles",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecepiesHaveTags",
                columns: table => new
                {
                    recipeId = table.Column<int>(type: "int", nullable: false),
                    tagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecepiesHaveTags", x => new { x.recipeId, x.tagId });
                    table.ForeignKey(
                        name: "FK_RecepiesHaveTags_recipes_recipeId",
                        column: x => x.recipeId,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecepiesHaveTags_tags_tagId",
                        column: x => x.tagId,
                        principalTable: "tags",
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
                    ingredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipesIncludeIngredients", x => x.id);
                    table.ForeignKey(
                        name: "FK_RecipesIncludeIngredients_recipes_ingredientId",
                        column: x => x.ingredientId,
                        principalTable: "recipes",
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
                        name: "FK_UsersLikeRecipes_recipes_recipeId",
                        column: x => x.recipeId,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersLikeRecipes_userProfiles_userId",
                        column: x => x.userId,
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
                name: "IX_last24Hs_userProfileId",
                table: "last24Hs",
                column: "userProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecepiesHaveTags_tagId",
                table: "RecepiesHaveTags",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_last24hid",
                table: "recipes",
                column: "last24hid");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_oilId",
                table: "recipes",
                column: "oilId");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_userProfileid",
                table: "recipes",
                column: "userProfileid");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIncludeIngredients_ingredientId",
                table: "RecipesIncludeIngredients",
                column: "ingredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIncludeIngredients_recipeId",
                table: "RecipesIncludeIngredients",
                column: "recipeId");

            migrationBuilder.CreateIndex(
                name: "IX_userProfiles_authOfProfileId",
                table: "userProfiles",
                column: "authOfProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userProfiles_dietid",
                table: "userProfiles",
                column: "dietid");

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
                name: "RecepiesHaveTags");

            migrationBuilder.DropTable(
                name: "RecipesIncludeIngredients");

            migrationBuilder.DropTable(
                name: "UsersLikeRecipes");

            migrationBuilder.DropTable(
                name: "UsersPreferTags");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "last24Hs");

            migrationBuilder.DropTable(
                name: "oils");

            migrationBuilder.DropTable(
                name: "userProfiles");

            migrationBuilder.DropTable(
                name: "auths");

            migrationBuilder.DropTable(
                name: "diets");
        }
    }
}
