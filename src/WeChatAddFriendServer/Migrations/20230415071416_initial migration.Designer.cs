﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace WeChatAddFriendServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230415071416_initial migration")]
    partial class initialmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("AppVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsForceUpdate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PatchFileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PatchSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PatchUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PatchVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AppVersions");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
