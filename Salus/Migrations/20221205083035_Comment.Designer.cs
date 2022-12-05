﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Salus.Data;

#nullable disable

namespace Salus_temp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221205083035_Comment")]
    partial class Comment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Salus.Controllers.Models.AuthModels.Auth", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<DateTime?>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("passwordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime?>("resetTokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("verificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("auths");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Comment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("body")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("fromId")
                        .HasColumnType("int");

                    b.Property<string>("sendDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("followedId", "followerId");

                    b.HasIndex("followerId");

                    b.ToTable("followings");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("authOfProfileId")
                        .HasColumnType("int");

                    b.Property<string>("birthDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("eyesIndex")
                        .HasColumnType("int");

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.Property<double>("goalWeight")
                        .HasColumnType("float");

                    b.Property<int>("hairIndex")
                        .HasColumnType("int");

                    b.Property<double>("height")
                        .HasColumnType("float");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<int>("mouthIndex")
                        .HasColumnType("int");

                    b.Property<int>("skinIndex")
                        .HasColumnType("int");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.HasIndex("authOfProfileId")
                        .IsUnique();

                    b.ToTable("userProfiles");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Comment", b =>
                {
                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "commentFrom")
                        .WithMany("commenterUserProfileToUserProfiles")
                        .HasForeignKey("fromId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "commentTo")
                        .WithMany("commentedUserProfileToUserProfiles")
                        .HasForeignKey("toId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("commentFrom");

                    b.Navigation("commentTo");
                });

            modelBuilder.Entity("Salus.Controllers.Models.JoiningEntity.Following", b =>
                {
                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "followed")
                        .WithMany("followedUserProfileToUserProfiles")
                        .HasForeignKey("followedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Salus.Controllers.Models.UserProfileModels.UserProfile", "follower")
                        .WithMany("followerUserProfileToUserProfiles")
                        .HasForeignKey("followerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("followed");

                    b.Navigation("follower");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.HasOne("Salus.Controllers.Models.AuthModels.Auth", "auth")
                        .WithOne("userProfile")
                        .HasForeignKey("Salus.Controllers.Models.UserProfileModels.UserProfile", "authOfProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("auth");
                });

            modelBuilder.Entity("Salus.Controllers.Models.AuthModels.Auth", b =>
                {
                    b.Navigation("userProfile");
                });

            modelBuilder.Entity("Salus.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.Navigation("commentedUserProfileToUserProfiles");

                    b.Navigation("commenterUserProfileToUserProfiles");

                    b.Navigation("followedUserProfileToUserProfiles");

                    b.Navigation("followerUserProfileToUserProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
