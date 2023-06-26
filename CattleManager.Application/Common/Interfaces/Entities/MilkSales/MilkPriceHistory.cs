namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public class MilkPriceHistory
{
    public DateOnly From { get; set; }
    public DateOnly? To { get; set; }
    public decimal Price { get; set; }
}