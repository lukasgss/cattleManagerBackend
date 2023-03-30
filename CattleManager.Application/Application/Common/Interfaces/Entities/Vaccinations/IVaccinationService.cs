namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public interface IVaccinationService
{
    Task<VaccinationResponse> GetVaccinationByIdAsync(Guid vaccinationId);
    Task<IEnumerable<VaccinationResponse>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId);
    Task<VaccinationResponse> CreateVaccinationAsync(CreateVaccinationRequest vaccinationRequest, Guid userId);
    Task<VaccinationResponse> EditVaccinationAsync(EditVaccinationRequest vaccinationRequest, Guid routeId, Guid userId);
    Task DeleteVaccinationAsync(Guid vaccinationId, Guid userId);
}