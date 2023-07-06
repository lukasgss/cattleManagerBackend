using CattleManager.Application.Application.Common.Constants;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class VaccinationRepository : GenericRepository<Vaccination>, IVaccinationRepository
{
    private readonly AppDbContext _dbContext;
    public VaccinationRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public double GetAmountOfPages(Guid cattleId, Guid userId)
    {
        return Math.Ceiling(_dbContext.Vaccinations.Where(x => x.CattleId == cattleId
            && x.Cattle.Users.Any(x => x.Id == userId)).Count() / (double)GlobalConstants.ResultsPerPage);
    }

    public async Task<IEnumerable<Vaccination>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId, int page)
    {
        return await _dbContext.Vaccinations
            .Include(x => x.Cattle.Users)
            .Where(x => x.CattleId == cattleId && x.Cattle.Users.Any(x => x.Id == userId))
            .OrderByDescending(x => x.Date)
            .Skip((page - 1) * GlobalConstants.ResultsPerPage)
            .Take(GlobalConstants.ResultsPerPage)
            .ToListAsync();
    }

    public async Task<Vaccination?> GetVaccinationByIdAsync(Guid vaccinationId)
    {
        return await _dbContext.Vaccinations.AsNoTracking().SingleOrDefaultAsync(x => x.Id == vaccinationId);
    }
}