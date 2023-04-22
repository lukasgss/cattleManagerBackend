using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleService
{
    Task<PaginatedCattleResponse> GetAllCattleFromOwner(Guid ownerId, int page);
    Task<CattleResponse> GetCattleById(Guid cattleId, Guid userId);
    Task<AmountOfCattleInLactationPeriod> GetAmountOfCattleInLactationPeriodAsync(Guid userId);
    Task<IEnumerable<CattleResponse>> GetCattleByNameAsync(string cattleName, Guid userId);
    Task<CattleResponse> CreateCattle(CattleRequest cattleRequest, Guid userId);
    Task<CattleResponse> EditCattle(EditCattleRequest cattleRequest, Guid userId, Guid routeId);
    Task<IEnumerable<DropdownData>> GetMaleCattleByName(string name, Guid userId);
    Task<IEnumerable<DropdownData>> GetFemaleCattleByName(string name, Guid userId);
    Task<IEnumerable<CattleResponse>> GetAllChildrenFromCattle(Guid cattleId, Guid userId);
    Task DeleteCattle(Guid cattleId, Guid userId);
}