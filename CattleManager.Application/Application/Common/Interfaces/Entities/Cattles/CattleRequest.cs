using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public record CattleRequest : ICattleRequest
{
#pragma warning disable 8618
    public CattleRequest() { }

    public Guid? Id { get; init; }
    public string Name { get; init; }
    public Guid? FatherId { get; init; }
    public Guid? MotherId { get; init; }
    public byte SexId { get; init; }
    public bool IsInLactationPeriod { get; set; }
    public IEnumerable<CattleBreedRequest> Breeds { get; init; }
    public DateOnly? PurchaseDate { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public int YearOfBirth { get; init; }
    public string? Image { get; init; }
    public DateOnly? DateOfDeath { get; init; }
    public string? CauseOfDeath { get; init; }
    public DateOnly? DateOfSale { get; init; }
    public int? PriceInCentsInReais { get; init; }
    public IEnumerable<Guid> OwnersIds { get; init; }
}
