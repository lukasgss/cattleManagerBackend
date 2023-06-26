using CattleManager.Application.Application.Common.Interfaces.Dashboard;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public class MilkProductionByMonth : IDataByMonth
{
    public DateOnly Date { get; set; }
    public decimal Value { get; set; }
}