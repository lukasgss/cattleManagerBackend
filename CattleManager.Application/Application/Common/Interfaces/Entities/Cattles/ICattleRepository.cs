using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleRepository : IGenericRepository<Cattle>
{
    Task<IEnumerable<Cattle>> GetCattleByName(string cattleName, Guid userId);
    Task<Cattle?> GetCattleById(Guid cattleId, Guid ownerId, bool trackChanges = true);
    Task<IEnumerable<Cattle>> GetAllCattlesFromOwner(Guid ownerId);
    Task<Cattle?> GetCattleDataOnlyById(Guid cattleId, Guid userId);
}