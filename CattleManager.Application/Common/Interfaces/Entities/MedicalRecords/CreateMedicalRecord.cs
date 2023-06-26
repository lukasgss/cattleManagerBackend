using CattleManager.Application.Application.Common.Enums;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public record CreateMedicalRecord(
    string Description,
    DateOnly Date,
    HealthCondition Type,
    string? Location,
    Guid CattleId);