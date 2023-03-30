namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public interface IConceptionService
{
    Task<ConceptionResponse> GetConceptionByIdAsync(Guid conceptionId);
    Task<IEnumerable<ConceptionResponse>> GetAllConceptionsFromCattleAsync(Guid cattleId, Guid userId);
    Task<ConceptionResponse> CreateConceptionAsync(CreateConceptionRequest conceptionRequest, Guid userId);
    Task<ConceptionResponse> EditConceptionAsync(EditConceptionRequest conceptionRequest, Guid userId, Guid routeId);
    Task DeleteConceptionAsync(Guid conceptionId, Guid userId);
}