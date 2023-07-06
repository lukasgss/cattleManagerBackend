global using Microsoft.EntityFrameworkCore;
using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Domain.Entities;
using CattleManager.Domain.Entities;

namespace CattleManager.Application.Infrastructure.Persistence.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cattle> Cattle { get; set; } = null!;
    public DbSet<CattleOwner> CattleOwners { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Vaccine> Vaccines { get; set; } = null!;
    public DbSet<Vaccination> Vaccinations { get; set; } = null!;
    public DbSet<Conception> Conceptions { get; set; } = null!;
    public DbSet<MilkProduction> MilkProductions { get; set; } = null!;
    public DbSet<Breed> Breeds { get; set; } = null!;
    public DbSet<CattleBreed> CattleBreeds { get; set; } = null!;
    public DbSet<MilkSale> MilkSales { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;
    public DbSet<Farm> Farms { get; set; } = null!;
    public DbSet<FarmMember> FarmMembers { get; set; } = null!;
    public DbSet<FarmOwner> FarmOwners { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cattle>(entity =>
        {
            entity.Property(x => x.Image).HasDefaultValue("https://i.imgur.com/xxNaPZH.png");
            entity.HasOne(x => x.Father).WithMany(x => x.FatherChildren)
                .HasForeignKey(x => x.FatherId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.Mother).WithMany(x => x.MotherChildren)
                .HasForeignKey(x => x.MotherId).OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(x => x.Users).WithMany(x => x.Cattle).UsingEntity<CattleOwner>();
            entity.HasMany(x => x.Conceptions).WithOne(x => x.Father);

            entity.ToTable(t => t.HasCheckConstraint("CK_Cattle_SexId", @"""Cattle"".""SexId"" = 0 OR ""Cattle"".""SexId"" = 1"));
        });

        modelBuilder.Entity<MilkProduction>(entity =>
            entity.ToTable(t =>
                t.HasCheckConstraint("CK_MilkProduction_PeriodOfDay", @"""MilkProductions"".""PeriodOfDay"" = 'm' OR ""MilkProductions"".""PeriodOfDay"" = 'a' OR ""MilkProductions"".""PeriodOfDay"" = 'n' OR ""MilkProductions"".""PeriodOfDay"" = 'd'")));

        modelBuilder.Entity<Breed>(entity =>
                {
                    entity.HasAlternateKey(x => x.Name);

                    entity.HasMany(x => x.Cattle).WithMany(x => x.Breeds).UsingEntity<CattleBreed>(
                        "CattleBreed",
                        cb => cb.HasOne(prop => prop.Cattle).WithMany(x => x.CattleBreeds).HasForeignKey(prop => prop.CattleId),
                        cb => cb.HasOne(prop => prop.Breed).WithMany(x => x.CattleBreeds).HasForeignKey(prop => prop.BreedId),
                        cb =>
                        {
                            cb.ToTable(x => x.HasCheckConstraint("QuantityBetween0And1", @"""CattleBreed"".""QuantityInPercentage"" >= 0 AND ""CattleBreed"".""QuantityInPercentage"" <= 1"));
                            cb.Property(prop => prop.QuantityInPercentage).IsRequired()
                            .HasColumnType("decimal(6, 5)");
                        }
                    );
                });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasAlternateKey(x => x.Name)
            .HasName("uniqueVaccineName");
        });

        modelBuilder.Entity<FarmMember>(entity =>
        {
            entity.HasKey(x => new { x.FarmId, x.MemberId });
            entity.HasOne(x => x.Farm).WithMany(x => x.Members);
        });

        modelBuilder.Entity<FarmOwner>(entity =>
        {
            entity.HasKey(x => new { x.FarmId, x.OwnerId });
            entity.HasOne(x => x.Farm).WithMany(x => x.Owners);
        });
    }
}