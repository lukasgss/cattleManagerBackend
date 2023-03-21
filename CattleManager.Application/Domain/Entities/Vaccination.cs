using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class Vaccination
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal DosageInMl { get; set; }
    public DateOnly Date { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;

    public Guid CattleId { get; set; }

    [ForeignKey("VaccineId")]
    public virtual Vaccine Vaccine { get; set; } = null!;
    public Guid VaccineId { get; set; }
}