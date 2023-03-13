namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public interface ICattleService
{
    Task<IEnumerable<CattleResponse>> GetAllCattlesFromOwner(Guid ownerId);
    Task<CattleResponse> GetCattleById(Guid cattleId, Guid userId);
    Task<IEnumerable<CattleResponse>> GetCattleByNameAsync(string cattleName, Guid userId);
    Task<CattleResponse> CreateCattle(CattleRequest cattleRequest, Guid userId);
}