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

    public async Task<IEnumerable<MilkPriceHistory>> GetMilkPriceHistoryAsync(Guid userId)
    {
        var query = await _dbContext.MilkSales
            .AsNoTracking()
            .GroupBy(milkSale => milkSale.PricePerLiter)
            .ToListAsync();

        List<MilkSale> milkSales = query.ConvertAll(x => x.ToList())
            .SelectMany(milkSale => milkSale)
            .OrderByDescending(milkSale => milkSale.Date)
            .ToList();

        List<MilkPriceHistory> milkPriceHistory = new()
        {
            new MilkPriceHistory()
            {
                From = milkSales[0].Date,
                To = null,
                Price = milkSales[0].PricePerLiter
            }
        };
        for (int i = 1; i < milkSales.Count - 1; i++)
        {
            milkPriceHistory.Add(new MilkPriceHistory()
            {
                From = milkSales[i + 1].Date,
                To = milkSales[i].Date,
                Price = milkSales[i + 1].PricePerLiter
            });
        }

        return milkPriceHistory;
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

    public async Task<AverageOfEntity> GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(Guid userId, int month, int year)
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

    public async Task<IEnumerable<IEnumerable<MilkSaleByMonth>>> GetTotalRevenueInPreviousMonths(int previousMonths, Guid userId)
    {
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly startDate = endDate.AddMonths(-previousMonths);

        var query = await _dbContext.MilkSales
            .AsNoTracking()
            .Where(milkSale => milkSale.Date >= startDate && milkSale.Date <= endDate
                && milkSale.Owner.Id == userId)
            .Select(milkSale => new MilkSaleByMonth()
            {
                Date = milkSale.Date,
                Value = milkSale.MilkInLiters * milkSale.PricePerLiter
            })
            .GroupBy(milkSale => milkSale.Date.Month)
            .ToListAsync();

        return query.ConvertAll(x => x.ToList());
    }
}