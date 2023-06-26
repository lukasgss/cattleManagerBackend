using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;

public interface IMilkSaleRepository : IGenericRepository<MilkSale>
{
    Task<IEnumerable<MilkSale>> GetAllMilkSalesAsync(Guid userId);
    Task<MilkSale?> GetMilkSaleById(Guid milkSaleId, Guid userId, bool trackChanges = true);
    Task<AverageOfEntity> GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(Guid userId, int month, int year);
    Task<IEnumerable<IEnumerable<MilkSaleByMonth>>> GetTotalRevenueInPreviousMonths(int previousMonths, Guid userId);
    Task<IEnumerable<MilkPriceHistory>> GetMilkPriceHistoryAsync(Guid userId);
}