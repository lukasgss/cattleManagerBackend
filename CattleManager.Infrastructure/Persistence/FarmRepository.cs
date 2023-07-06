using CattleManager.Application.Common.Interfaces.Entities.Farms;
using CattleManager.Application.Infrastructure.Persistence;
using CattleManager.Application.Infrastructure.Persistence.DataContext;
using CattleManager.Domain.Entities;

namespace CattleManager.Infrastructure.Persistence;

public class FarmRepository : GenericRepository<Farm>, IFarmRepository
{
    private readonly AppDbContext _dbContext;

    public FarmRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Farm?> GetFarmByIdAsync(Guid userId, Guid farmId, bool trackChanges = true)
    {
        return trackChanges ? (await _dbContext.Farms
            .Include(farm => farm.Owners)
            .FirstOrDefaultAsync(farm => farm.Id == farmId
                && farm.Owners.Any(owner => owner.OwnerId == userId)))
            :
            (await _dbContext.Farms
                .AsNoTracking()
            .Include(farm => farm.Owners)
            .FirstOrDefaultAsync(farm => farm.Id == farmId
                && farm.Owners.Any(owner => owner.OwnerId == userId)));
    }
}