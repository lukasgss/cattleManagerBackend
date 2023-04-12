﻿// <auto-generated />
using System;
using CattleManager.Application.Infrastructure.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CattleManager.Application.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Cattle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CauseOfDeath")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

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

                    b.HasIndex("Name");

                    b.ToTable("Cattle", null, t =>
                        {
                            t.HasCheckConstraint("CK_Cattle_SexId", "\"Cattle\".\"SexId\" = 0 OR \"Cattle\".\"SexId\" = 1");
                        });
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

                    b.Property<decimal>("MilkInLiters")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<string>("PeriodOfDay")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.ToTable("MilkProductions", null, t =>
                        {
                            t.HasCheckConstraint("CK_MilkProduction_PeriodOfDay", "\"MilkProductions\".\"PeriodOfDay\" = 'morning' OR \"MilkProductions\".\"PeriodOfDay\" = 'afternoon' OR \"MilkProductions\".\"PeriodOfDay\" = 'night' OR \"MilkProductions\".\"PeriodOfDay\" = 'whole day'");
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

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("DosageInMl")
                        .HasColumnType("decimal(9, 4)");

                    b.Property<Guid>("VaccineId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.HasIndex("VaccineId");

                    b.ToTable("Vaccinations");
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

                    b.HasAlternateKey("Name")
                        .HasName("uniqueVaccineName");

                    b.ToTable("Vaccines");
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

                    b.Navigation("Father");

                    b.Navigation("Mother");
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccination", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany("Vaccinations")
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.Vaccine", "Vaccine")
                        .WithMany("Vaccinations")
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

                    b.Navigation("Vaccinations");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccine", b =>
                {
                    b.Navigation("Vaccinations");
                });
#pragma warning restore 612, 618
        }
    }
}
