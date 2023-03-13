using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public record CattleRequest(
    string Name,
    Guid? FatherId,
    Guid? MotherId,
    byte SexId,
    IEnumerable<CattleBreedRequest> Breeds,
    DateOnly? PurchaseDate,
    DateOnly? ConceptionDate,
    DateOnly? DateOfBirth,
    int YearOfBirth,
    string? Image,
    DateOnly? DateOfDeath,
    string? CauseOfDeath,
    DateOnly? DateOfSale,
    int? PriceInCentsInReais,
    IEnumerable<Guid> OwnersIds);