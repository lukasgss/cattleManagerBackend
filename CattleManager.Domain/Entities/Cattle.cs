using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CatetleManager.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CattleManager.Application.Domain.Entities;

[Table("Cattle")]
[Index(nameof(Name))]
public class Cattle
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    // Year of birth is stored independently in case date of birth is not known,
    // with date of birth being null and only year of birth being stored. Otherwise, both are stored

    [Required]
    public int YearOfBirth { get; set; }

    [MaxLength(1000)]
    public string? Image { get; set; }
    public DateOnly? DateOfDeath { get; set; }

    [MaxLength(255)]
    public string? CauseOfDeath { get; set; }
    public DateOnly? DateOfSale { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public int? PriceInCentsInReais { get; set; }

    [Required]
    public byte SexId { get; set; }

    [Required]
    public bool IsInLactationPeriod { get; set; }

    public virtual ICollection<User> Users { get; set; } = null!;
    public virtual ICollection<CattleOwner> CattleOwners { get; set; } = null!;
    public virtual ICollection<Cattle> MotherChildren { get; set; } = null!;
    public virtual ICollection<Cattle> FatherChildren { get; set; } = null!;

    [ForeignKey("FatherId")]
    public virtual Cattle? Father { get; set; }
    public Guid? FatherId { get; set; }

    [ForeignKey("MotherId")]
    public virtual Cattle? Mother { get; set; }
    public Guid? MotherId { get; set; }
    public virtual ICollection<Breed> Breeds { get; set; } = null!;
    public virtual ICollection<CattleBreed> CattleBreeds { get; set; } = null!;
    public virtual ICollection<Vaccination> Vaccinations { get; set; } = null!;
    public virtual ICollection<MilkProduction> MilkProductions { get; set; } = null!;
    public virtual ICollection<Conception> Conceptions { get; set; } = null!;
}