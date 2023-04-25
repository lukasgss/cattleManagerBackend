using CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class MilkSaleRepository : GenericRepository<MilkSale>, IMilkSaleRepository
{
    private readonly AppDbContext _dbContext;
    public MilkSaleRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MilkSale>> GetAllMilkSalesAsync(Guid userId)
    {
        return await _dbContext.MilkSales
        .AsNoTracking()
        .Where(milkSale => milkSale.Owner.Id == userId)
        .OrderByDescending(milkSale => milkSale.Date)
        .ToListAsync();
    }

    public async Task<MilkSale?> GetMilkSaleById(Guid milkSaleId, Guid userId, bool trackChanges = true)
    {
        return trackChanges ? await _dbContext.MilkSales
        .SingleOrDefaultAsync(milkSale => milkSale.Id == milkSaleId && milkSale.Owner.Id == userId)
        :
        await _dbContext.MilkSales
        .AsNoTracking()
        .SingleOrDefaultAsync(milkSale => milkSale.Id == milkSaleId && milkSale.Owner.Id == userId);
    }

    public async Task<AverageOfEntity> GetMilkSalesAverageTotalIncomeInSpecificMonthAsync(Guid userId, int month, int year)
    {
        DateOnly startDate = new(year, month, 1);
        DateOnly endDate = startDate.AddDays(DateTime.DaysInMonth(year, month) - 1);

        var query = await _dbContext.MilkSales
            .AsNoTracking()
            .Where(milkSale => milkSale.Date >= startDate && milkSale.Date <= endDate
                && milkSale.Owner.Id == userId)
            .ToListAsync();

        decimal average = query.Average(x => x.MilkInLiters * x.PricePerLiter);
        int amountOfSales = query.Select(x => x.Id).Count();

        return new AverageOfEntity()
        {
            Average = average,
            Quantity = amountOfSales
        };
    }
}