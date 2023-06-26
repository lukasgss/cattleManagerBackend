namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public record MilkProductionResponse(
    Guid Id,
    string CattleName,
    decimal MilkInLiters,
    string PeriodOfDay,
    string Date,
    Guid CattleId);