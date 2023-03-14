﻿// <auto-generated />
using System;
using CattleManager.Application.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CattleManager.Application.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230309212648_FixCattleImageColumnDefaultValue")]
    partial class FixCattleImageColumnDefaultValue
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CatetleManager.Application.Domain.Entities.Conception", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("FatherId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MotherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FatherId");

                    b.HasIndex("MotherId");

                    b.ToTable("Conceptions", (string)null);
                });

            modelBuilder.Entity("CattleBreed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BreedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("QuantityInPercentage")
                        .HasColumnType("decimal(6, 5)");

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.HasIndex("CattleId");

                    b.ToTable("CattleBreed", t =>
                        {
                            t.HasCheckConstraint("QuantityBetween0And1", "\"CattleBreed\".\"QuantityInPercentage\" >= 0 AND \"CattleBreed\".\"QuantityInPercentage\" <= 1");
                        });
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Breeds", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("7cd24abf-3139-4eff-aee5-720853c33649"),
                            Name = "Gir"
                        },
                        new
                        {
                            Id = new Guid("1d22d7af-e864-4c3c-9e49-351e98bc2b45"),
                            Name = "Holandês"
                        },
                        new
                        {
                            Id = new Guid("7a5ff6db-5be0-401f-90cf-eadccaa9ac55"),
                            Name = "Jersey"
                        },
                        new
                        {
                            Id = new Guid("2d4de1ea-6b8b-47cb-bab6-e7ad6325ae26"),
                            Name = "Pardo Suíço"
                        },
                        new
                        {
                            Id = new Guid("a4ab1168-fed4-450a-8a03-ec746f30f5e0"),
                            Name = "Guzerá"
                        },
                        new
                        {
                            Id = new Guid("5db28616-aa0c-4818-9e1c-214e0e07fe40"),
                            Name = "Nelore"
                        },
                        new
                        {
                            Id = new Guid("2eb00e47-7522-4c0f-94bd-a21e2ea1c2bb"),
                            Name = "Simental"
                        },
                        new
                        {
                            Id = new Guid("17f01d8f-ba82-4d3e-bd9d-4164533de3e2"),
                            Name = "Sindi"
                        },
                        new
                        {
                            Id = new Guid("6c4d3582-b300-485d-ae6c-a6b26f437ae2"),
                            Name = "Brahman"
                        });
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Cattle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CauseOfDeath")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateOnly?>("ConceptionDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DateOfDeath")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DateOfSale")
                        .HasColumnType("date");

                    b.Property<Guid?>("FatherId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasDefaultValue("https://i.imgur.com/xxNaPZH.png");

                    b.Property<Guid?>("MotherId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int?>("PriceInCentsInReais")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("PurchaseDate")
                        .HasColumnType("date");

                    b.Property<byte>("SexId")
                        .HasColumnType("smallint");

                    b.Property<int>("YearOfBirth")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FatherId");

                    b.HasIndex("MotherId");

                    b.HasIndex("SexId");

                    b.ToTable("Cattle", (string)null);
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.CattleOwner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.HasIndex("UserId");

                    b.ToTable("CattleOwners");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MilkProduction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("MilkPerDayInLiters")
                        .HasColumnType("decimal(6, 2)");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.ToTable("MilkProductions", (string)null);
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Sex", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Gender");

                    b.ToTable("Sex", null, t =>
                        {
                            t.HasCheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1");
                        });

                    b.HasData(
                        new
                        {
                            Id = (byte)0,
                            Gender = "Fêmea"
                        },
                        new
                        {
                            Id = (byte)1,
                            Gender = "Macho"
                        });
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Vaccines", (string)null);
                });

            modelBuilder.Entity("Vaccinations", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateOnly(2023, 3, 9));

                    b.Property<decimal>("DosageInMl")
                        .HasColumnType("decimal(9, 4)");

                    b.Property<Guid>("VaccineId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.HasIndex("VaccineId");

                    b.ToTable("Vaccinations");
                });

            modelBuilder.Entity("CatetleManager.Application.Domain.Entities.Conception", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Father")
                        .WithMany("Conceptions")
                        .HasForeignKey("FatherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Mother")
                        .WithMany()
                        .HasForeignKey("MotherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Father");

                    b.Navigation("Mother");
                });

            modelBuilder.Entity("CattleBreed", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Breed", "Breed")
                        .WithMany("CattleBreeds")
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany("CattleBreeds")
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Breed");

                    b.Navigation("Cattle");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Cattle", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Father")
                        .WithMany("FatherChildren")
                        .HasForeignKey("FatherId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Mother")
                        .WithMany("MotherChildren")
                        .HasForeignKey("MotherId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CattleManager.Application.Domain.Entities.Sex", "Sex")
                        .WithMany()
                        .HasForeignKey("SexId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Father");

                    b.Navigation("Mother");

                    b.Navigation("Sex");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.CattleOwner", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany("CattleOwners")
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cattle");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MilkProduction", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany("MilkProductions")
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cattle");
                });

            modelBuilder.Entity("Vaccinations", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany()
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.Vaccine", "Vaccine")
                        .WithMany()
                        .HasForeignKey("VaccineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cattle");

                    b.Navigation("Vaccine");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Breed", b =>
                {
                    b.Navigation("CattleBreeds");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Cattle", b =>
                {
                    b.Navigation("CattleBreeds");

                    b.Navigation("CattleOwners");

                    b.Navigation("Conceptions");

                    b.Navigation("FatherChildren");

                    b.Navigation("MilkProductions");

                    b.Navigation("MotherChildren");
                });
#pragma warning restore 612, 618
        }
    }
}