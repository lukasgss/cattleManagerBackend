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
    [Migration("20230302213526_FixTableUsers")]
    partial class FixTableUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid?>("MotherId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Owner", b =>
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

                    b.ToTable("Owners", (string)null);
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

                    b.ToTable("Sex", null, t =>
                        {
                            t.HasCheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1");
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

                    b.ToTable("Vaccinations", (string)null);
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

                    b.ToTable("Vaccines", (string)null);
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.MilkProduction", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany()
                        .HasForeignKey("CattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cattle");
                });

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Owner", b =>
                {
                    b.HasOne("CattleManager.Application.Domain.Entities.Cattle", "Cattle")
                        .WithMany()
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Vaccination", b =>
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

            modelBuilder.Entity("CattleManager.Application.Domain.Entities.Cattle", b =>
                {
                    b.Navigation("FatherChildren");

                    b.Navigation("MotherChildren");
                });
#pragma warning restore 612, 618
        }
    }
}
