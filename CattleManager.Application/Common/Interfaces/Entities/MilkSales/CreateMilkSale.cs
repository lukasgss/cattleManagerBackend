namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public record CreateMilkSale(decimal MilkInLiters, decimal PricePerLiter, DateOnly Date);