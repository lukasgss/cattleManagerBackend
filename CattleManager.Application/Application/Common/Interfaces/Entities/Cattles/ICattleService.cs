namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleService
{
    Task<IEnumerable<CattleResponse>> GetAllCattleFromOwner(Guid ownerId, int page);
    Task<CattleResponse> GetCattleById(Guid cattleId, Guid userId);
    Task<IEnumerable<CattleResponse>> GetCattleByNameAsync(string cattleName, Guid userId);
    Task<CattleResponse> CreateCattle(CattleRequest cattleRequest, Guid userId);
    Task<CattleResponse> EditCattle(EditCattleRequest cattleRequest, Guid userId, Guid routeId);
    Task DeleteCattle(Guid cattleId, Guid userId);
}