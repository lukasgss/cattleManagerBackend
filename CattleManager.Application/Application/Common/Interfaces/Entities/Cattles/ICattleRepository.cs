using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleRepository : IGenericRepository<Cattle>
{
    double GetAmountOfPages(Guid userId);
    Task<IEnumerable<Cattle>> GetCattleByName(string cattleName, Guid userId);
    Task<Cattle?> GetCattleBySpecificName(string cattleName, Guid userId);
    Task<Cattle?> GetCattleById(Guid cattleId, Guid ownerId, bool trackChanges = true);
    Task<IEnumerable<Cattle>> GetAllCattleFromOwner(Guid ownerId, int page);
    Task<Cattle?> GetCattleDataOnlyById(Guid cattleId, Guid userId);
    Task<IEnumerable<DropdownData>> GetMaleCattleByName(string name, Guid userId);
    Task<IEnumerable<DropdownData>> GetFemaleCattleByName(string name, Guid userId);
    Task<IEnumerable<DropdownData>> GetAllCattleByNameForDropdownAsync(string name, Guid userId);
    Task<IEnumerable<Cattle>> GetAllChildrenFromCattleFromSpecificGenderAsync(Guid cattleId, Guid userId, Gender gender);
    Task<IEnumerable<Cattle>> GetAllChildrenFromCattleAsync(Guid cattleId, Guid userId);
    Task<Cattle?> GetCattleIdAndSexByCattleIdAsync(Guid cattleId, Guid userId);
    Task<int> GetAmountOfCattleInLactationPeriodAsync(Guid userId);
    Task<int> GetAmountOfCattleInDryPeriodAsync(Guid userId);
}