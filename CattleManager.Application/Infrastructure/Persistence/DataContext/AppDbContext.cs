using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Infrastructure.Persistence.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cattle> Cattles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Owner> Owners { get; set; } = null!;
    public DbSet<Vaccine> Vaccines { get; set; } = null!;
    public DbSet<Vaccination> Vaccinations { get; set; } = null!;
    public DbSet<MilkProduction> MilkProductions { get; set; } = null!;
    public DbSet<Sex> Sex { get; set; } = null!;

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
            entity.Property(x => x.Image).HasMaxLength(1000);
            entity.Property(x => x.SexId).IsRequired();
            entity.Property(x => x.CauseOfDeath).HasMaxLength(255);
            entity.Property(x => x.DateOfSale);
            entity.HasOne(x => x.Father).WithMany(x => x.FatherChildren)
                .HasForeignKey(x => x.FatherId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.Mother).WithMany(x => x.MotherChildren)
                .HasForeignKey(x => x.MotherId).OnDelete(DeleteBehavior.SetNull);
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

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("Owners");
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.CattleId).IsRequired();
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.ToTable("Vaccines");
            entity.Property(x => x.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.ToTable("Vaccinations");
            entity.Property(x => x.DosageInMl).IsRequired().HasColumnType("decimal(9, 4)");
            entity.Property(x => x.Date).IsRequired();
            entity.Property(x => x.CattleId).IsRequired();
            entity.Property(x => x.VaccineId).IsRequired();
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
            entity.ToTable(x => x.HasCheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1"));
            entity.Property(x => x.Gender).IsRequired().HasMaxLength(20);
        });
    }
}