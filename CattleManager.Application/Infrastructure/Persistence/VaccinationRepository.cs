using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class VaccinationRepository : GenericRepository<Vaccination>, IVaccinationRepository
{
    private readonly AppDbContext _dbContext;
    public VaccinationRepository(AppDbContext dbContext, AppDbContext dbContext2) : base(dbContext)
    {
        _dbContext = dbContext2;
    }

    public async Task<IEnumerable<Vaccination>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId)
    {
        return await _dbContext.Vaccinations
        .Include(x => x.Cattle.Users)
        .Where(x => x.CattleId == cattleId && x.Cattle.Users.Any(x => x.Id == userId))
        .ToListAsync();
    }

    public async Task<Vaccination?> GetVaccinationByIdAsync(Guid vaccinationId)
    {
        return await _dbContext.Vaccinations.AsNoTracking().SingleOrDefaultAsync(x => x.Id == vaccinationId);
    }
}