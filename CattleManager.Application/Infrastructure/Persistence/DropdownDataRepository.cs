using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class DropdownDataRepository : IDropdownDataRepository
{
    private readonly AppDbContext _dbContext;
    public DropdownDataRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<DropdownDataResponse>> GetMaleCattleByName(string name, Guid userId)
    {
        return await _dbContext.Cattle
        .AsNoTracking()
        .Where(x => (x.SexId == (int)Gender.Male)
            && x.Name.ToLower().Contains(name.ToLowerInvariant())
            && x.Users.Any(x => x.Id == userId))
        .Select(x => new DropdownDataResponse() { Text = x.Name, Value = x.Id })
        .ToListAsync();
    }
}