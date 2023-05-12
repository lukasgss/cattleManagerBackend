using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;

public interface IMilkProductionRepository : IGenericRepository<MilkProduction>
{
    double GetAmountOfPages(Guid cattleId, Guid userId);
    double GetAmountOfPages(Guid userId);
    Task<MilkProduction?> GetMilkProductionByIdAsync(Guid milkProductionId, Guid userId, bool trackChanges = true);
    Task<IEnumerable<MilkProduction>> GetAllMilkProductionsAsync(Guid userId, int page);
    Task<IEnumerable<MilkProduction>> GetMilkProductionsFromCattleAsync(Guid cattleId, Guid userid, int page);
    Task<AverageOfEntity> GetAverageMilkProductionFromAllCattleAsync(Guid userId, int month, int year);
    Task<AverageMilkProduction> GetAverageMilkProductionFromCattleAsync(Guid cattleId, Guid userId, int month, int year);
    Task<IEnumerable<IEnumerable<MilkProductionByMonth>>> GetTotalMilkProductionLastMonthsAsync(int previousMonths, Guid userId);
}