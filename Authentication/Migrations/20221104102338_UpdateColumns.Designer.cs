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
    [Migration("20221104102338_UpdateColumns")]
    partial class UpdateColumns
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

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.Property<double>("goalWeight")
                        .HasColumnType("float");

                    b.Property<double>("height")
                        .HasColumnType("float");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.HasIndex("authOfProfileId")
                        .IsUnique();

                    b.ToTable("userProfiles");
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
                    b.Navigation("userProfile")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
