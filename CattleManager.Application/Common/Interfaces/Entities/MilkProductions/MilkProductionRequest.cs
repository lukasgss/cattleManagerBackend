namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record MilkProductionRequest(decimal MilkInLiters, string PeriodOfDay, DateOnly Date, Guid CattleId);