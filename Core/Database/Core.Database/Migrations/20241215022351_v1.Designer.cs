﻿// <auto-generated />
using System;
using Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Core.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241215022351_v1")]
    partial class v1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Database.Models.Account.AccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Core.Database.Models.Player.PlayerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountModelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATE");

                    b.Property<int>("Diamonds")
                        .HasColumnType("INT");

                    b.Property<int>("Experience")
                        .HasColumnType("INT");

                    b.Property<int>("Golds")
                        .HasColumnType("INT");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("DATE");

                    b.Property<int>("Level")
                        .HasColumnType("INT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR");

                    b.Property<byte>("SlotNumber")
                        .HasColumnType("TINYINT");

                    b.HasKey("Id");

                    b.HasIndex("AccountModelId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Player", (string)null);
                });

            modelBuilder.Entity("Core.Database.Models.Player.PlayerModel", b =>
                {
                    b.HasOne("Core.Database.Models.Account.AccountModel", "AccountModel")
                        .WithMany("Players")
                        .HasForeignKey("AccountModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Core.Database.Models.Player.Position", "Position", b1 =>
                        {
                            b1.Property<int>("PlayerModelId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<double>("Rotation")
                                .HasColumnType("float");

                            b1.Property<double>("X")
                                .HasColumnType("FLOAT");

                            b1.Property<double>("Y")
                                .HasColumnType("FLOAT");

                            b1.Property<int>("Z")
                                .HasColumnType("int");

                            b1.HasKey("PlayerModelId");

                            b1.ToTable("Player");

                            b1.WithOwner("PlayerModel")
                                .HasForeignKey("PlayerModelId");

                            b1.Navigation("PlayerModel");
                        });

                    b.OwnsOne("Core.Database.Models.Player.Stats", "Stats", b1 =>
                        {
                            b1.Property<int>("PlayerModelId")
                                .HasColumnType("int");

                            b1.Property<int>("Agility")
                                .HasColumnType("INT");

                            b1.Property<int>("Defense")
                                .HasColumnType("INT");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<int>("Intelligence")
                                .HasColumnType("INT");

                            b1.Property<int>("Strength")
                                .HasColumnType("INT");

                            b1.Property<int>("Willpower")
                                .HasColumnType("INT");

                            b1.HasKey("PlayerModelId");

                            b1.ToTable("Player");

                            b1.WithOwner("PlayerModel")
                                .HasForeignKey("PlayerModelId");

                            b1.Navigation("PlayerModel");
                        });

                    b.OwnsOne("Core.Database.Models.Player.Vitals", "Vitals", b1 =>
                        {
                            b1.Property<int>("PlayerModelId")
                                .HasColumnType("int");

                            b1.Property<int>("Health")
                                .HasColumnType("INT");

                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<int>("Mana")
                                .HasColumnType("INT");

                            b1.Property<double>("MaxHealth")
                                .HasColumnType("float");

                            b1.Property<double>("MaxMana")
                                .HasColumnType("float");

                            b1.HasKey("PlayerModelId");

                            b1.ToTable("Player");

                            b1.WithOwner("PlayerModel")
                                .HasForeignKey("PlayerModelId");

                            b1.Navigation("PlayerModel");
                        });

                    b.Navigation("AccountModel");

                    b.Navigation("Position")
                        .IsRequired();

                    b.Navigation("Stats")
                        .IsRequired();

                    b.Navigation("Vitals")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Database.Models.Account.AccountModel", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}