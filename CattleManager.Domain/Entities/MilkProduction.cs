using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class MilkProduction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column(TypeName = "decimal(5, 2)")]
    public decimal MilkInLiters { get; set; }

    [Required]
    public char PeriodOfDay { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}