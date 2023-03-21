using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccines;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class VaccineRepository : GenericRepository<Vaccine>, IVaccineRepository
{
    private readonly AppDbContext _dbContext;

    public VaccineRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Vaccine?> GetVaccineByIdAsync(Guid vaccineId)
    {
        return await _dbContext.Vaccines.SingleOrDefaultAsync(x => x.Id == vaccineId);
    }
}