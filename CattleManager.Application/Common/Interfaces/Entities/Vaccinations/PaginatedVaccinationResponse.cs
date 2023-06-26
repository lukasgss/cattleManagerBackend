namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public record PaginatedVaccinationResponse(IEnumerable<VaccinationResponse> Vaccinations, int CurrentPage, double Pages);