using CattleManager.Application.Application.Common.Interfaces.DecimalToFraction;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;

public record CattleBreedResponse(string? Breed, Fraction? QuantityInFraction);