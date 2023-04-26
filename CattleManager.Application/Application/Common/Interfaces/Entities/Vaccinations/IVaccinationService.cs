namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public interface IVaccinationService
{
    Task<VaccinationResponse> GetVaccinationByIdAsync(Guid vaccinationId);
    Task<PaginatedVaccinationResponse> GetAllVaccinationsFromCattleAsync(Guid cattleId, Guid userId, int page);
    Task<VaccinationResponse> CreateVaccinationAsync(CreateVaccinationRequest vaccinationRequest, Guid userId);
    Task<VaccinationResponse> EditVaccinationAsync(EditVaccinationRequest vaccinationRequest, Guid routeId, Guid userId);
    Task DeleteVaccinationAsync(Guid vaccinationId, Guid userId);
}