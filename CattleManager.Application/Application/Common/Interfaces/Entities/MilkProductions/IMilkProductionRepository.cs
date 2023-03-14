using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public interface IMilkProductionRepository : IGenericRepository<MilkProduction>
{
    Task<MilkProduction?> GetMilkProductionByIdAsync(Guid milkProductionId, Guid userId, bool trackChanges = true);
    Task<IEnumerable<MilkProduction>> GetMilkProductionsFromCattleAsync(Guid cattleId, Guid userid);
}