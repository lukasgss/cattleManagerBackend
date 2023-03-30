namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public record PaginatedCattleResponse(IEnumerable<CattleResponse> Cattle, int CurrentPage, double Pages);