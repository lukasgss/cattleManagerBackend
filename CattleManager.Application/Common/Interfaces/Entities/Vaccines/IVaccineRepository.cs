using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccines;

public interface IVaccineRepository
{
    Task<Vaccine?> GetVaccineByIdAsync(Guid vaccineId);
}