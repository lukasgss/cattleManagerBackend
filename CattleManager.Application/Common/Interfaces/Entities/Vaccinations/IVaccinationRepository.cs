using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public interface IVaccinationRepository : IGenericRepository<Vaccination>
{
    double GetAmountOfPages(Guid cattleId, Guid userId);
    public Task<IEnumerable<Vaccination>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId, int page);
    public Task<Vaccination?> GetVaccinationByIdAsync(Guid vaccinationId);
}