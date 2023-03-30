namespace CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;

public record PaginatedConceptionResponse(IEnumerable<ConceptionResponse> Conceptions, int CurrentPage, double Pages);