using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Application.Common.Interfaces.GenericRepository;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public interface IConceptionRepository : IGenericRepository<Conception>
{
    Task<IEnumerable<Conception>> GetAllConceptionsFromCattle(Guid cattleId, Guid userId);
    Task<Conception?> GetConceptionByIdAsync(Guid id, bool trackChanges = true);
}