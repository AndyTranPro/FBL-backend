﻿// <auto-generated />
using System;
using FooBooLooGameAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FooBooLooGameAPI.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20240824160150_UpdateGameTable")]
    partial class UpdateGameTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FooBooLooGameAPI.Entities.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Max")
                        .HasColumnType("integer");

                    b.Property<int>("Min")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RuleSet")
                        .HasColumnType("jsonb");

                    b.HasKey("GameId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("FooBooLooGameAPI.Entities.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionId"));

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsEnded")
                        .HasColumnType("boolean");

                    b.Property<string>("PlayerName")
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SessionId");

                    b.HasIndex("GameId", "PlayerName", "StartTime")
                        .IsUnique();

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("FooBooLooGameAPI.Entities.SessionNumber", b =>
                {
                    b.Property<int>("SessionNumberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionNumberId"));

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("NumberServed")
                        .HasColumnType("integer");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.HasKey("SessionNumberId");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionNumbers");
                });

            modelBuilder.Entity("FooBooLooGameAPI.Entities.Session", b =>
                {
                    b.HasOne("FooBooLooGameAPI.Entities.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("FooBooLooGameAPI.Entities.SessionNumber", b =>
                {
                    b.HasOne("FooBooLooGameAPI.Entities.Session", "Session")
                        .WithMany("SessionNumbers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("FooBooLooGameAPI.Entities.Session", b =>
                {
                    b.Navigation("SessionNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
