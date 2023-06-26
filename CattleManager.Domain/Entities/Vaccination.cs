using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class Vaccination
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column(TypeName = "decimal(9, 4)")]
    public decimal DosageInMl { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;

    public Guid CattleId { get; set; }

    [ForeignKey("VaccineId")]
    public virtual Vaccine Vaccine { get; set; } = null!;
    public Guid VaccineId { get; set; }
}