﻿// <auto-generated />
using System;
using Auth_API.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Auth_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220304103017_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Auth_API.Models.Dto.User.UserDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Uuid");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Auth_API.Models.Dto.User.UserTokensDto", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClientIp")
                        .HasColumnType("varchar(45)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RefreshTokenExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SpotifyRefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserUuid")
                        .HasColumnType("char(36)");

                    b.HasKey("Uuid");

                    b.HasIndex("UserUuid")
                        .IsUnique();

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Auth_API.Models.Dto.User.UserTokensDto", b =>
                {
                    b.HasOne("Auth_API.Models.Dto.User.UserDto", null)
                        .WithOne("SpotifyAccountData")
                        .HasForeignKey("Auth_API.Models.Dto.User.UserTokensDto", "UserUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auth_API.Models.Dto.User.UserDto", b =>
                {
                    b.Navigation("SpotifyAccountData");
                });
#pragma warning restore 612, 618
        }
    }
}
