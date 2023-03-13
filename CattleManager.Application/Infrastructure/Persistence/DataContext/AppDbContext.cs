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
    public DbSet<MilkProduction> MilkProductions { get; set; } = null!;
    public DbSet<Sex> Sex { get; set; } = null!;
    public DbSet<Breed> Breeds { get; set; } = null!;
    public DbSet<CattleBreed> CattleBreeds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cattle>(entity =>
        {
            entity.ToTable("Cattle");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(255);
            entity.Property(x => x.PurchaseDate);
            entity.Property(x => x.ConceptionDate);
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

            entity.HasMany(x => x.Vaccines).WithMany(x => x.Cattle).UsingEntity<Vaccination>(
                "Vaccinations",
                vac => vac.HasOne(prop => prop.Vaccine).WithMany().HasForeignKey(prop => prop.VaccineId),
                vac => vac.HasOne(prop => prop.Cattle).WithMany().HasForeignKey(prop => prop.CattleId),
                vac =>
                {
                    vac.Property(prop => prop.DosageInMl).IsRequired().HasColumnType("decimal(9, 4)");
                    vac.Property(prop => prop.Date).IsRequired().HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));
                }
            );

            entity.HasMany(x => x.Conceptions).WithOne(x => x.Father);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Username).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(255);
            entity.Property(x => x.Password).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.ToTable("Vaccines");
            entity.HasAlternateKey(x => x.Name);
            entity.Property(x => x.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MilkProduction>(entity =>
        {
            entity.ToTable("MilkProductions");
            entity.Property(x => x.MilkPerDayInLiters).IsRequired().HasColumnType("decimal(6, 2)");
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.CattleId).IsRequired();
        });

        modelBuilder.Entity<Sex>(entity =>
        {
            entity.ToTable("Sex");
            entity.HasAlternateKey(x => x.Gender);
            entity.ToTable(x => x.HasCheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1"));
            entity.Property(x => x.Gender).IsRequired().HasMaxLength(20);
            entity.HasData(
                new Sex
                {
                    Id = 0,
                    Gender = "Fêmea"
                },
                new Sex
                {
                    Id = 1,
                    Gender = "Macho"
                }
            );
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

            entity.HasData(
                new Breed { Id = Guid.NewGuid(), Name = "Gir" },
                new Breed { Id = Guid.NewGuid(), Name = "Holandês" },
                new Breed { Id = Guid.NewGuid(), Name = "Jersey" },
                new Breed { Id = Guid.NewGuid(), Name = "Pardo Suíço" },
                new Breed { Id = Guid.NewGuid(), Name = "Guzerá" },
                new Breed { Id = Guid.NewGuid(), Name = "Nelore" },
                new Breed { Id = Guid.NewGuid(), Name = "Simental" },
                new Breed { Id = Guid.NewGuid(), Name = "Sindi" },
                new Breed { Id = Guid.NewGuid(), Name = "Brahman" }
            );
        });

        modelBuilder.Entity<Conception>(entity =>
        {
            entity.ToTable("Conceptions");
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.FatherId).IsRequired();
            entity.Property(x => x.MotherId).IsRequired();
        });
    }
}