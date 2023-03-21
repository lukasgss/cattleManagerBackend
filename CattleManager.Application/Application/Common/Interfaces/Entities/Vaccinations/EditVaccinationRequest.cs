namespace CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;

public record EditVaccinationRequest(Guid Id, Guid CattleId, Guid VaccineId, decimal DosageInMl, DateOnly Date);