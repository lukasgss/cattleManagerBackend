using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleRequest
{
    Guid? Id { get; init; }
    string Name { get; init; }
    Guid? FatherId { get; init; }
    Guid? MotherId { get; init; }
    byte SexId { get; init; }
    IEnumerable<CattleBreedRequest> Breeds { get; init; }
    DateOnly? PurchaseDate { get; init; }
    DateOnly? DateOfBirth { get; init; }
    int YearOfBirth { get; init; }
    string? Image { get; init; }
    DateOnly? DateOfDeath { get; init; }
    string? CauseOfDeath { get; init; }
    DateOnly? DateOfSale { get; init; }
    int? PriceInCentsInReais { get; init; }
    IEnumerable<Guid> OwnersIds { get; init; }
}