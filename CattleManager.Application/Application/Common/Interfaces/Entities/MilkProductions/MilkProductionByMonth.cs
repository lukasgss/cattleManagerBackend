namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public class MilkProductionByMonth
{
    public DateOnly Date { get; set; }
    public decimal MilkInLiters { get; set; }
}