namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public record VaccinationResponse(Guid Id, Guid CattleId, Guid VaccineId, decimal DosageInMl, DateOnly Date);