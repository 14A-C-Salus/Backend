﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Salus.Data;

#nullable disable

namespace Salus.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230331101201_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Salus.Controllers.Models.AuthModels.Auth", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("passwordResetToken")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<DateTime?>("resetTokenExpires")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("verificationToken")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("auths");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Comment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("body")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<int>("fromId")
                        .HasColumnType("int");

                    b.Property<string>("sendDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("toId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("fromId");

                    b.HasIndex("toId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Following", b =>
                {
                    b.Property<int>("followedId")
                        .HasColumnType("int");

                    b.Property<int>("followerId")
                        .HasColumnType("int");

                    b.Property<string>("followDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("followedId", "followerId");

                    b.HasIndex("followerId");

                    b.ToTable("followings");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.RecepiesHaveTags", b =>
                {
                    b.Property<int>("recipeId")
                        .HasColumnType("int");

                    b.Property<int>("tagId")
                        .HasColumnType("int");

                    b.HasKey("recipeId", "tagId");

                    b.HasIndex("tagId");

                    b.ToTable("RecepiesHaveTags");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.RecipesIncludeIngredients", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ingredientId")
                        .HasColumnType("int");

                    b.Property<int>("portionInGramm")
                        .HasColumnType("int");

                    b.Property<int>("recipeId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ingredientId");

                    b.HasIndex("recipeId");

                    b.ToTable("RecipesIncludeIngredients");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.UsersLikeRecipes", b =>
                {
                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<int>("recipeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("userId", "recipeId");

                    b.HasIndex("recipeId");

                    b.ToTable("UsersLikeRecipes");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.UsersPreferTags", b =>
                {
                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.Property<int>("tagId")
                        .HasColumnType("int");

                    b.HasKey("userId", "tagId");

                    b.HasIndex("tagId");

                    b.ToTable("UsersPreferTags");
                });

            modelBuilder.Entity("Salus.Controllers.Models.Last24hModels.Last24h", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("carbohydrate")
                        .HasColumnType("int");

                    b.Property<int>("fat")
                        .HasColumnType("int");

                    b.Property<int>("kcal")
                        .HasColumnType("int");

                    b.Property<int?>("liquidInDl")
                        .HasColumnType("int");

                    b.Property<int>("protein")
                        .HasColumnType("int");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("userProfileId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userProfileId")
                        .IsUnique();

                    b.ToTable("last24Hs");
                });

            modelBuilder.Entity("Salus.Controllers.Models.RecipeModels.Oil", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("calIn14Ml")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("oils");
                });

            modelBuilder.Entity("Salus.Controllers.Models.RecipeModels.Recipe", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("carbohydrate")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("fat")
                        .HasColumnType("int");

                    b.Property<int>("gramm")
                        .HasColumnType("int");

                    b.Property<int>("kcal")
                        .HasColumnType("int");

                    b.Property<int?>("last24hid")
                        .HasColumnType("int");

                    b.Property<int>("method")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("oilId")
                        .HasColumnType("int");

                    b.Property<int?>("oilPortionMl")
                        .HasColumnType("int");

                    b.Property<int>("protein")
                        .HasColumnType("int");

                    b.Property<int>("timeInMinute")
                        .HasColumnType("int");

                    b.Property<bool>("verifeid")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("id");

                    b.HasIndex("last24hid");

                    b.HasIndex("oilId");

                    b.ToTable("recipes");
                });

            modelBuilder.Entity("Salus.Controllers.Models.TagModels.Tag", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("max")
                        .HasColumnType("int");

                    b.Property<int?>("min")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("recipeProperty")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("authOfProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("birthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("dietid")
                        .HasColumnType("int");

                    b.Property<int>("eyesIndex")
                        .HasColumnType("int");

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.Property<double>("goalWeight")
                        .HasColumnType("double");

                    b.Property<int>("hairIndex")
                        .HasColumnType("int");

                    b.Property<double>("height")
                        .HasColumnType("double");

                    b.Property<int?>("maxKcal")
                        .HasColumnType("int");

                    b.Property<int?>("minDl")
                        .HasColumnType("int");

                    b.Property<int>("mouthIndex")
                        .HasColumnType("int");

                    b.Property<int>("skinIndex")
                        .HasColumnType("int");

                    b.Property<double>("weight")
                        .HasColumnType("double");

                    b.HasKey("id");

                    b.HasIndex("authOfProfileId")
                        .IsUnique();

                    b.HasIndex("dietid");

                    b.ToTable("userProfiles");
                });

            modelBuilder.Entity("Salus.Models.Diet", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("maxCarbohydrate")
                        .HasColumnType("int");

                    b.Property<int?>("maxDl")
                        .HasColumnType("int");

                    b.Property<int?>("maxFat")
                        .HasColumnType("int");

                    b.Property<int?>("maxKcal")
                        .HasColumnType("int");

                    b.Property<int?>("maxProtein")
                        .HasColumnType("int");

                    b.Property<int?>("minCarbohydrate")
                        .HasColumnType("int");

                    b.Property<int?>("minDl")
                        .HasColumnType("int");

                    b.Property<int?>("minFat")
                        .HasColumnType("int");

                    b.Property<int?>("minKcal")
                        .HasColumnType("int");

                    b.Property<int?>("minProtein")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("diets");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Comment", b =>
                {
                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "commentFrom")
                        .WithMany("commenteds")
                        .HasForeignKey("fromId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "commentTo")
                        .WithMany("commenters")
                        .HasForeignKey("toId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("commentFrom");

                    b.Navigation("commentTo");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Following", b =>
                {
                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "followed")
                        .WithMany("followers")
                        .HasForeignKey("followedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "follower")
                        .WithMany("followeds")
                        .HasForeignKey("followerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("followed");

                    b.Navigation("follower");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.RecepiesHaveTags", b =>
                {
                    b.HasOne("Salus.Controllers.Models.RecipeModels.Recipe", "recipe")
                        .WithMany("tags")
                        .HasForeignKey("recipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.TagModels.Tag", "tag")
                        .WithMany("recepiesThatHave")
                        .HasForeignKey("tagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("recipe");

                    b.Navigation("tag");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.RecipesIncludeIngredients", b =>
                {
                    b.HasOne("Salus.Controllers.Models.RecipeModels.Recipe", "ingredient")
                        .WithMany("recipes")
                        .HasForeignKey("ingredientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.RecipeModels.Recipe", "recipe")
                        .WithMany("ingredients")
                        .HasForeignKey("recipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ingredient");

                    b.Navigation("recipe");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.UsersLikeRecipes", b =>
                {
                    b.HasOne("Salus.Controllers.Models.RecipeModels.Recipe", "recipe")
                        .WithMany("usersWhoLiked")
                        .HasForeignKey("recipeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "user")
                        .WithMany("likedRecipes")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("recipe");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.UsersPreferTags", b =>
                {
                    b.HasOne("Salus.Controllers.Models.TagModels.Tag", "tag")
                        .WithMany("usersWhoPrefer")
                        .HasForeignKey("tagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "user")
                        .WithMany("preferredTags")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("tag");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Salus.Controllers.Models.Last24hModels.Last24h", b =>
                {
                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "userProfile")
                        .WithOne("last24h")
                        .HasForeignKey("Salus.Controllers.Models.Last24hModels.Last24h", "userProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("userProfile");
                });

            modelBuilder.Entity("Salus.Controllers.Models.RecipeModels.Recipe", b =>
                {
                    b.HasOne("Salus.Controllers.Models.Last24hModels.Last24h", "last24h")
                        .WithMany("recipes")
                        .HasForeignKey("last24hid");

                    b.HasOne("Salus.Controllers.Models.RecipeModels.Oil", "oil")
                        .WithMany("recipes")
                        .HasForeignKey("oilId");

                    b.Navigation("last24h");

                    b.Navigation("oil");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.HasOne("Salus.Controllers.Models.AuthModels.Auth", "auth")
                        .WithOne("userProfile")
                        .HasForeignKey("Salus.Controllers.Models.UserProfileModels.UserProfile", "authOfProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salus.Models.Diet", "diet")
                        .WithMany("userProfiles")
                        .HasForeignKey("dietid");

                    b.Navigation("auth");

                    b.Navigation("diet");
                });

            modelBuilder.Entity("Salus.Controllers.Models.AuthModels.Auth", b =>
                {
                    b.Navigation("userProfile");
                });

            modelBuilder.Entity("Salus.Controllers.Models.Last24hModels.Last24h", b =>
                {
                    b.Navigation("recipes");
                });

            modelBuilder.Entity("Salus.Controllers.Models.RecipeModels.Oil", b =>
                {
                    b.Navigation("recipes");
                });

            modelBuilder.Entity("Salus.Controllers.Models.RecipeModels.Recipe", b =>
                {
                    b.Navigation("ingredients");

                    b.Navigation("recipes");

                    b.Navigation("tags");

                    b.Navigation("usersWhoLiked");
                });

            modelBuilder.Entity("Salus.Controllers.Models.TagModels.Tag", b =>
                {
                    b.Navigation("recepiesThatHave");

                    b.Navigation("usersWhoPrefer");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.Navigation("commenteds");

                    b.Navigation("commenters");

                    b.Navigation("followeds");

                    b.Navigation("followers");

                    b.Navigation("last24h");

                    b.Navigation("likedRecipes");

                    b.Navigation("preferredTags");
                });

            modelBuilder.Entity("Salus.Models.Diet", b =>
                {
                    b.Navigation("userProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}