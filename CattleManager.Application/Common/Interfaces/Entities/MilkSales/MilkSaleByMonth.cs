using CattleManager.Application.Application.Common.Interfaces.Dashboard;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public class MilkSaleByMonth : IDataByMonth
{
    public DateOnly Date { get; set; }
    public decimal Value { get; set; }
}