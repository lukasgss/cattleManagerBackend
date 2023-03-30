using CattleManager.Application.Application.Common.Constants;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class CattleRepository : GenericRepository<Cattle>, ICattleRepository
{
    private readonly AppDbContext _dbContext;
    public CattleRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Cattle>> GetCattleByName(string cattleName, Guid userId)
    {
        return await _dbContext.Cattle
        .Include(x => x.Users)
        .Include(x => x.CattleOwners)
            .ThenInclude(x => x.User)
        .Include(x => x.Breeds)
        .Include(x => x.CattleBreeds)
            .ThenInclude(x => x.Breed)
        .Include(x => x.Sex)
        .Where(cattle =>
            cattle.Name.ToLower().Contains(cattleName.ToLowerInvariant())
            && cattle.Users.Any(user => user.Id == userId))
        .ToListAsync();
    }

    public async Task<IEnumerable<Cattle>> GetAllCattleFromOwner(Guid ownerId, int page)
    {
        return await _dbContext.Cattle
        .Include(x => x.CattleOwners)
            .ThenInclude(x => x.User)
        .Include(x => x.Breeds)
        .Include(x => x.CattleBreeds)
            .ThenInclude(x => x.Breed)
        .Include(x => x.Sex)
        .Where(x => x.Users.Any(x => x.Id == ownerId))
        .OrderBy(x => x.Name)
        .Skip((page - 1) * GlobalConstants.ResultsPerPage)
        .Take(GlobalConstants.ResultsPerPage)
        .ToListAsync();
    }

    public async Task<Cattle?> GetCattleDataOnlyById(Guid cattleId, Guid userId)
    {
        return await _dbContext.Cattle
            .Include(x => x.Vaccinations)
            .SingleOrDefaultAsync(x => x.Id == cattleId && x.Users.Any(x => x.Id == userId));
    }

    public async Task<Cattle?> GetCattleById(Guid cattleId, Guid userId, bool trackChanges = false)
    {
        return trackChanges ? (await _dbContext.Cattle
            .Include(x => x.CattleOwners)
                .ThenInclude(x => x.User)
            .Include(x => x.CattleBreeds)
                .ThenInclude(x => x.Breed)
            .Include(x => x.Sex)
            .Include(x => x.Vaccinations)
            .SingleOrDefaultAsync(x => x.Id == cattleId && x.Users.Any(x => x.Id == userId)))
            :
            (await _dbContext.Cattle
            .Include(x => x.CattleOwners)
                .ThenInclude(x => x.User)
            .Include(x => x.CattleBreeds)
                .ThenInclude(x => x.Breed)
            .Include(x => x.Sex)
            .Include(x => x.Vaccinations)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == cattleId && x.Users.Any(x => x.Id == userId)));
    }
}