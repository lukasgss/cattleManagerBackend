namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record EditMilkProductionRequest(Guid Id, decimal MilkInLiters, string PeriodOfDay, DateOnly Date, Guid CattleId);