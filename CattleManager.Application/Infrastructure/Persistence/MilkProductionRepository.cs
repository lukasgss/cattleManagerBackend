using CattleManager.Application.Application.Common.Constants;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class MilkProductionRepository : GenericRepository<MilkProduction>, IMilkProductionRepository
{
    private readonly AppDbContext _dbContext;

    public MilkProductionRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public double GetAmountOfPages(Guid cattleId, Guid userId)
    {
        return Math.Ceiling(_dbContext.MilkProductions.Where(x => x.CattleId == cattleId
            && x.Cattle.Users.Any(x => x.Id == userId)).Count() / (double)GlobalConstants.ResultsPerPage);
    }

    public async Task<AverageOfEntity> GetMilkProductionAverageFromAllCattle(Guid userId, int month, int year)
    {
        DateOnly startDate = new(year, month, 1);
        DateOnly endDate = startDate.AddDays(DateTime.DaysInMonth(year, month) - 1);

        List<MilkProduction> query = await _dbContext.MilkProductions
            .AsNoTracking()
            .Where(milkProduction => milkProduction.Cattle.Users.Any(user => user.Id == userId)
                && milkProduction.Date >= startDate && milkProduction.Date <= endDate)
            .ToListAsync();

        if (query.Count == 0)
        {
            return new AverageOfEntity()
            {
                Average = 0,
                Quantity = 0,
            };
        }

        decimal milkAverage = query.GroupBy(mp => mp.Date)
            .Select(x => x.ToList())
            .ToList()
            .Average(x => x.Sum(s => s.MilkInLiters));

        int amountOfAnimals = query.Select(x => x.CattleId).Distinct().Count();

        return new AverageOfEntity()
        {
            Average = milkAverage,
            Quantity = amountOfAnimals
        };
    }

    public async Task<MilkProduction?> GetMilkProductionByIdAsync(Guid milkProductionId, Guid userId, bool trackChanges = true)
    {
        return trackChanges ? (await _dbContext.MilkProductions
        .AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == milkProductionId && x.Cattle.Users.Any(x => x.Id == userId)))
        :
        (await _dbContext.MilkProductions
        .SingleOrDefaultAsync(x => x.Id == milkProductionId && x.Cattle.Users.Any(x => x.Id == userId)));
    }

    public async Task<IEnumerable<MilkProduction>> GetMilkProductionsFromCattleAsync(Guid cattleId, Guid userId, int page)
    {
        return await _dbContext.MilkProductions
        .Where(x => x.CattleId == cattleId && x.Cattle.Users.Any(x => x.Id == userId))
        .OrderByDescending(x => x.Date)
        .Skip((page - 1) * GlobalConstants.ResultsPerPage)
        .Take(GlobalConstants.ResultsPerPage)
        .ToListAsync();
    }
}