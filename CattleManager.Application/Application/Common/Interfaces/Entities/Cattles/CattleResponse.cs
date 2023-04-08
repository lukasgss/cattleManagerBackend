using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Owners;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public record CattleResponse(
    Guid Id,
    string Name,
    Guid? FatherId,
    string? FatherName,
    Guid? MotherId,
    string? MotherName,
    string Sex,
    IEnumerable<CattleBreedResponse> CattleBreeds,
    DateOnly? PurchaseDate,
    DateOnly? DateOfBirth,
    int YearOfBirth,
    string? Image,
    DateOnly? DateOfDeath,
    string? CauseOfDeath,
    DateOnly? DateOfSale,
    IEnumerable<CattleOwnerResponse> Owners);