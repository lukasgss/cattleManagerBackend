namespace CattleManager.Application.Common.Interfaces.Entities.Farms;

public interface IFarmService
{
    Task<FarmResponse> GetFarmByIdAsync(Guid userId, Guid farmId);
    Task<FarmResponse> CreateFarmAsync(Guid userId, CreateFarmRequest createFarmRequest);
    Task<FarmResponse> EditFarmAsync(Guid userId, EditFarmRequest editFarmRequest, Guid routeId);
    Task DeleteFarmAsync(Guid farmId, Guid userId);
}