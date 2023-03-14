namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record EditMilkProductionRequest(Guid Id, decimal MilkPerDayInLiters, DateOnly Date, Guid CattleId);