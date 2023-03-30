using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public interface IMilkProductionService
{
    Task<MilkProductionResponse> GetMilkProductionByIdAsync(Guid id, Guid userId);
    Task<PaginatedMilkProductionResponse> GetAllMilkProductionsFromCattleAsync(Guid cattleId, Guid userId, int page);
    Task<MilkProductionResponse> CreateMilkProductionAsync(MilkProductionRequest milkProductionRequest, Guid userId);
    Task<MilkProductionResponse> EditMilkProductionByIdAsync(
        EditMilkProductionRequest editedMilkProduction,
        Guid userId,
        Guid routeId);
    Task DeleteMilkProductionByIdAsync(Guid milkProductionId, Guid userId);
}