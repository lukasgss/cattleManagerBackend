using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public interface IVaccinationRepository : IGenericRepository<Vaccination>
{
    public Task<IEnumerable<Vaccination>> GetAllVaccinationsFromCattle(Guid cattleId, Guid userId);
    public Task<Vaccination?> GetVaccinationByIdAsync(Guid vaccinationId);
}