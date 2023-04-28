using CattleManager.Application.Application.Common.Enums;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public class MedicalRecordResponse
{
    public Guid Id { get; set; }
    public Guid CattleId { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly Date { get; set; }
    public HealthCondition Type { get; set; }
    public string? Location { get; set; }
}