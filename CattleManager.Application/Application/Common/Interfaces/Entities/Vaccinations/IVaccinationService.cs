namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public interface IVaccinationService
{
    Task<VaccinationResponse> GetVaccinationById(Guid vaccinationId);
    Task<IEnumerable<VaccinationResponse>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId);
    Task<VaccinationResponse> CreateVaccination(CreateVaccinationRequest vaccinationRequest, Guid userId);
    Task<VaccinationResponse> EditVaccination(EditVaccinationRequest vaccinationRequest, Guid routeId, Guid userId);
}