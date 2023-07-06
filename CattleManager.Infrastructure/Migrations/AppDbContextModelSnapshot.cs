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
                .HasAnnotation("ProductVersion", "7.0.8")
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

                    b.ToTable("Conceptions");
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

                    b.ToTable("Breeds");
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
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasDefaultValue("https://i.imgur.com/xxNaPZH.png");

                    b.Property<bool>("IsInLactationPeriod")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("MotherId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<decimal?>("PriceInCentsInReais")
                        .HasColumnType("decimal(12, 2)");

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

                    b.ToTable("Cattle", t =>
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MedicalRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CattleId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Location")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.ToTable("MedicalRecords");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("HasBeenRead")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
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
                        .HasColumnType("decimal(5, 2)");

                    b.Property<char>("PeriodOfDay")
                        .HasColumnType("character(1)");

                    b.HasKey("Id");

                    b.HasIndex("CattleId");

                    b.ToTable("MilkProductions", t =>
                        {
                            t.HasCheckConstraint("CK_MilkProduction_PeriodOfDay", "\"MilkProductions\".\"PeriodOfDay\" = 'm' OR \"MilkProductions\".\"PeriodOfDay\" = 'a' OR \"MilkProductions\".\"PeriodOfDay\" = 'n' OR \"MilkProductions\".\"PeriodOfDay\" = 'd'");
                        });
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MilkSale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("MilkInLiters")
                        .HasColumnType("decimal(7, 2)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("PricePerLiter")
                        .HasColumnType("decimal(12, 2)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("MilkSales");
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

                    b.ToTable("Users");
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

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Vaccines");
                });

            modelBuilder.Entity("CattleManager.Domain.Entities.Farm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("CattleManager.Domain.Entities.FarmMember", b =>
                {
                    b.Property<Guid>("FarmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.HasKey("FarmId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("FarmMembers");
                });

            modelBuilder.Entity("CattleManager.Domain.Entities.FarmOwner", b =>
                {
                    b.Property<Guid>("FarmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("FarmId", "OwnerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("FarmOwners");
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MedicalRecord", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany()
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cattle");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Message", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MilkSale", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
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

            modelBuilder.Entity("CattleManager.Domain.Entities.FarmMember", b =>
                {
                    b.HasOne("CattleManager.Domain.Entities.Farm", "Farm")
                        .WithMany("Members")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.User", "User")
                        .WithMany("FarmMember")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CattleManager.Domain.Entities.FarmOwner", b =>
                {
                    b.HasOne("CattleManager.Domain.Entities.Farm", "Farm")
                        .WithMany("Owners")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CattleManager.Application.Domain.Entities.User", "User")
                        .WithMany("OwnedFarm")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");

                    b.Navigation("User");
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.User", b =>
                {
                    b.Navigation("FarmMember");

                    b.Navigation("OwnedFarm");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccine", b =>
                {
                    b.Navigation("Vaccinations");
                });

            modelBuilder.Entity("CattleManager.Domain.Entities.Farm", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Owners");
                });
#pragma warning restore 612, 618
        }
    }
}