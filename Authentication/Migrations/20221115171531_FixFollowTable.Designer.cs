﻿// <auto-generated />
using System;
using Authentication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Authentication_temp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221115171531_FixFollowTable")]
    partial class FixFollowTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Authentication.Controllers.Models.AuthModels.Auth", b =>
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

            modelBuilder.Entity("Authentication.Controllers.Models.JoiningEntity.FollowUserProfile", b =>
                {
                    b.Property<int>("followId")
                        .HasColumnType("int");

                    b.Property<int>("userProfileId")
                        .HasColumnType("int");

                    b.HasKey("followId", "userProfileId");

                    b.HasIndex("userProfileId");

                    b.ToTable("followUserProfiles");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.SocialMediaModels.Follow", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("followedId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("follows");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.UserProfileModels.UserProfile", b =>
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

            modelBuilder.Entity("Authentication.Controllers.Models.JoiningEntity.FollowUserProfile", b =>
                {
                    b.HasOne("Authentication.Controllers.Models.SocialMediaModels.Follow", "follow")
                        .WithMany("followUserProfile")
                        .HasForeignKey("followId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Authentication.Controllers.Models.UserProfileModels.UserProfile", "userProfile")
                        .WithMany("followUserProfile")
                        .HasForeignKey("userProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("follow");

                    b.Navigation("userProfile");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.HasOne("Authentication.Controllers.Models.AuthModels.Auth", "auth")
                        .WithOne("userProfile")
                        .HasForeignKey("Authentication.Controllers.Models.UserProfileModels.UserProfile", "authOfProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("auth");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.AuthModels.Auth", b =>
                {
                    b.Navigation("userProfile");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.SocialMediaModels.Follow", b =>
                {
                    b.Navigation("followUserProfile");
                });

            modelBuilder.Entity("Authentication.Controllers.Models.UserProfileModels.UserProfile", b =>
                {
                    b.Navigation("followUserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
