using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CattleManager.Application.Application.Common.Enums;

namespace CattleManager.Application.Domain.Entities;

public class MedicalRecord
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = null!;

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public HealthCondition Type { get; set; }

    [MaxLength(100)]
    public string? Location { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}