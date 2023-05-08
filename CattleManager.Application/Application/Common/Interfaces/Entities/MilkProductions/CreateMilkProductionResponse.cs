namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record CreateMilkProductionResponse(
    Guid Id,
    decimal MilkInLiters,
    string PeriodOfDay,
    string Date,
    Guid CattleId
);