namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public record EditMilkSale(Guid Id, decimal MilkInLiters, decimal PricePerLiter, DateOnly Date);