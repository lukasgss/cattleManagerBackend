using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public interface IMilkSaleService
{
    Task<IEnumerable<MilkSaleResponse>> GetAllMilkSalesAsync(Guid userId);
    Task<MilkSaleResponse> GetMilkSaleByIdAsync(Guid milkSaleId, Guid userId);
    Task<AverageOfEntity> GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(Guid userId, int month, int year);
    Task<IEnumerable<DataInMonth<decimal>>> GetMilkSalesTotalRevenueInPreviousMonths(int previousMonths, Guid userId);
    Task<IEnumerable<MilkPriceHistory>> GetHistoryOfMilkPrices(Guid userId);
    Task<MilkSaleResponse> CreateMilkSaleAsync(CreateMilkSale createMilkSale, Guid userId);
    Task<MilkSaleResponse> EditMilkSaleAsync(EditMilkSale editMilkSale, Guid userId, Guid routeId);
    Task DeleteMilkSaleAsync(Guid milkSaleId, Guid userId);
}