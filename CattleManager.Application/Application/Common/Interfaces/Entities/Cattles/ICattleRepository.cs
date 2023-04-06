using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleRepository : IGenericRepository<Cattle>
{
    double GetAmountOfPages(Guid userId);
    Task<IEnumerable<Cattle>> GetCattleByName(string cattleName, Guid userId);
    Task<Cattle?> GetCattleById(Guid cattleId, Guid ownerId, bool trackChanges = true);
    Task<IEnumerable<Cattle>> GetAllCattleFromOwner(Guid ownerId, int page);
    Task<Cattle?> GetCattleDataOnlyById(Guid cattleId, Guid userId);
    Task<IEnumerable<DropdownDataResponse>> GetMaleCattleByName(string name, Guid userId);
    Task<IEnumerable<DropdownDataResponse>> GetFemaleCattleByName(string name, Guid userId);
}