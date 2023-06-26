namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public record CreateVaccinationRequest(Guid CattleId, Guid VaccineId, decimal DosageInMl, DateOnly Date);