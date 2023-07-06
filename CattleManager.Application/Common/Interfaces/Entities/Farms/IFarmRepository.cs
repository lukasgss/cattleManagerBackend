using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Domain.Entities;

namespace CattleManager.Application.Common.Interfaces.Entities.Farms;

public interface IFarmRepository : IGenericRepository<Farm>
{
    public Task<Farm?> GetFarmByIdAsync(Guid userId, Guid farmId, bool trackChanges = true);
}