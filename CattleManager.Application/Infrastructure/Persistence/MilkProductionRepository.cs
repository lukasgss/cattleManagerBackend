using CattleManager.Application.Application.Common.Constants;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
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