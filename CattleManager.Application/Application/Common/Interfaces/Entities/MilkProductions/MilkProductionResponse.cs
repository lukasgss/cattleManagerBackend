namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record MilkProductionResponse(Guid Id, decimal MilkPerDayInLiters, DateOnly Date, Guid CattleId);