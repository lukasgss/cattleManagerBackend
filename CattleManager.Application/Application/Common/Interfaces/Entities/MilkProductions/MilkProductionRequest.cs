namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record MilkProductionRequest(decimal MilkPerDayInLiters, DateOnly Date, Guid CattleId);