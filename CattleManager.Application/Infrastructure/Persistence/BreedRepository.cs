using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class BreedRepository : IBreedRepository
{
    private readonly AppDbContext _dbContext;

    public BreedRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DropdownDataResponse>> GetAllBreedsForDropdown()
    {
        return await _dbContext.Breeds
        .AsNoTracking()
        .Select(x => new DropdownDataResponse() { Text = x.Name, Value = x.Id })
        .ToListAsync();
    }
}