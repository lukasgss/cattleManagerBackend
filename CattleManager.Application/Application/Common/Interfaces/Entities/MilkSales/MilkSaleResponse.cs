namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public class MilkSaleResponse
{
    public Guid Id { get; set; }
    public decimal MilkInLiters { get; set; }
    public decimal PricePerLiter { get; set; }
    public decimal TotalPrice { get; set; }
    public DateOnly Date { get; set; }
}