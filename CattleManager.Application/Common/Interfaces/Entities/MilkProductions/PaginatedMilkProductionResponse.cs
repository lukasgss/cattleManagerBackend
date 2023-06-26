namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record PaginatedMilkProductionResponse(IEnumerable<MilkProductionResponse> MilkProductions, int CurrentPage, double Pages);