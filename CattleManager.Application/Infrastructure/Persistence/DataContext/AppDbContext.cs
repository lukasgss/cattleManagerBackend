using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Domain.Entities;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cattle>(entity =>
        {
            entity.ToTable("Cattle");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(255);
            entity.Property(x => x.PurchaseDate);
            entity.Property(x => x.DateOfBirth);
            entity.Property(x => x.YearOfBirth).IsRequired();
            entity.Property(x => x.Image).IsRequired().HasMaxLength(1000).HasDefaultValue("https://i.imgur.com/xxNaPZH.png");
            entity.Property(x => x.SexId).IsRequired();
            entity.Property(x => x.CauseOfDeath).HasMaxLength(255);
            entity.Property(x => x.DateOfSale);
            entity.Property(x => x.PriceInCentsInReais);
            entity.HasOne(x => x.Father).WithMany(x => x.FatherChildren)
                .HasForeignKey(x => x.FatherId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.Mother).WithMany(x => x.MotherChildren)
                .HasForeignKey(x => x.MotherId).OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(x => x.Users).WithMany(x => x.Cattle).UsingEntity<CattleOwner>();
            entity.HasMany(x => x.Conceptions).WithOne(x => x.Father);

            entity.ToTable(t => t.HasCheckConstraint("CK_Cattle_SexId", @"""Cattle"".""SexId"" = 0 OR ""Cattle"".""SexId"" = 1"));
        });
        modelBuilder.Entity<Cattle>().HasIndex(x => x.Name);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Password).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<MilkProduction>(entity =>
        {
            entity.ToTable("MilkProductions");
            entity.Property(x => x.MilkInLiters).IsRequired().HasColumnType("decimal(6, 2)");
            entity.Property(x => x.PeriodOfDay).IsRequired().HasMaxLength(9);
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.CattleId).IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_MilkProduction_PeriodOfDay", @"""MilkProductions"".""PeriodOfDay"" = 'morning' OR ""MilkProductions"".""PeriodOfDay"" = 'afternoon' OR ""MilkProductions"".""PeriodOfDay"" = 'night' OR ""MilkProductions"".""PeriodOfDay"" = 'whole day'"));
        });

        modelBuilder.Entity<Breed>(entity =>
        {
            entity.ToTable("Breeds");
            entity.HasAlternateKey(x => x.Name);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(50);

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

        modelBuilder.Entity<Conception>(entity =>
        {
            entity.ToTable("Conceptions");
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.FatherId).IsRequired();
            entity.Property(x => x.MotherId).IsRequired();
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasAlternateKey(x => x.Name)
            .HasName("uniqueVaccineName");
        });
    }
}