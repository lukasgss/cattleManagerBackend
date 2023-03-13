using CatetleManager.Application.Domain.Entities;

namespace CattleManager.Application.Domain.Entities;

public class Cattle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? ConceptionDate { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    // Year of birth is stored independently in case date of birth is not known,
    // with date of birth being null and only year of birth being stored. Otherwise, both are stored
    public int YearOfBirth { get; set; }
    public string? Image { get; set; }
    public DateOnly? DateOfDeath { get; set; }
    public string? CauseOfDeath { get; set; }
    public DateOnly? DateOfSale { get; set; }
    public int? PriceInCentsInReais { get; set; }

    public ICollection<User> Users { get; set; } = null!;
    public ICollection<CattleOwner> CattleOwners { get; set; } = null!;
    public ICollection<Cattle> MotherChildren { get; set; } = null!;
    public ICollection<Cattle> FatherChildren { get; set; } = null!;
    public Cattle? Father { get; set; }
    public Guid? FatherId { get; set; }
    public Cattle? Mother { get; set; }
    public Guid? MotherId { get; set; }
    public Sex Sex { get; set; } = null!;
    public byte SexId { get; set; }
    public ICollection<Breed> Breeds { get; set; } = null!;
    public ICollection<CattleBreed> CattleBreeds { get; set; } = null!;
    public ICollection<Vaccine> Vaccines { get; set; } = null!;
    public ICollection<MilkProduction> MilkProductions { get; set; } = null!;
    public ICollection<Conception> Conceptions { get; set; } = null!;
}