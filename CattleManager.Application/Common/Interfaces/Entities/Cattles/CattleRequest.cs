using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public record CattleRequest : ICattleRequest

    public Guid? Id { get; init; }
    public string Name { get; init; } = null!;
    public Guid? FatherId { get; init; }
    public Guid? MotherId { get; init; }
    public byte SexId { get; init; }
    public bool IsInLactationPeriod { get; init; }
    public IEnumerable<CattleBreedRequest> Breeds { get; init; } = null!;
    public DateOnly? PurchaseDate { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public int YearOfBirth { get; init; }
    public string? Image { get; init; }
    public DateOnly? DateOfDeath { get; init; }
    public string? CauseOfDeath { get; init; }
    public DateOnly? DateOfSale { get; init; }
    public int? PriceInCentsInReais { get; init; }
    public IEnumerable<Guid> OwnersIds { get; init; } = null!;
}
