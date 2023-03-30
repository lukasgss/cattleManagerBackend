using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class ConceptionRepository : GenericRepository<Conception>, IConceptionRepository
{
    private readonly AppDbContext _dbContext;

    public ConceptionRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Conception>> GetAllConceptionsFromCattle(Guid cattleId, Guid userId)
    {
        return await _dbContext.Conceptions
        .Where(x => (x.MotherId == cattleId && x.Mother.Users.Any(x => x.Id == userId)) ||
            (x.FatherId == cattleId && x.Father.Users.Any(x => x.Id == userId)))
        .ToListAsync();
    }

    public async Task<Conception?> GetConceptionByIdAsync(Guid id, bool trackChanges = true)
    {
        return trackChanges ? await _dbContext.Conceptions.SingleOrDefaultAsync(x => x.Id == id)
        :
        await _dbContext.Conceptions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }
}