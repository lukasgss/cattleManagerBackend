using CattleManager.Application.Application.Common.Enums;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public record EditMedicalRecords(
    Guid Id,
    string Description,
    DateOnly Date,
    HealthCondition Type,
    string? Location,
    Guid CattleId);