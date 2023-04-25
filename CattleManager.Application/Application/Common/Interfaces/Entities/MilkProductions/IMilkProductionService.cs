using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public interface IMilkProductionService
{
    Task<MilkProductionResponse> GetMilkProductionByIdAsync(Guid id, Guid userId);
    Task<PaginatedMilkProductionResponse> GetAllMilkProductionsFromCattleAsync(Guid cattleId, Guid userId, int page);
    Task<AverageOfEntity> GetAverageMilkProductionFromAllCattleAsync(Guid userId, int month, int year);
    Task<MilkProductionResponse> CreateMilkProductionAsync(MilkProductionRequest milkProductionRequest, Guid userId);
    Task<MilkProductionResponse> EditMilkProductionByIdAsync(
        EditMilkProductionRequest editedMilkProduction,
        Guid userId,
        Guid routeId);
    Task DeleteMilkProductionByIdAsync(Guid milkProductionId, Guid userId);
}