using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CattleManager.Application.Domain.Entities;

namespace CatetleManager.Application.Domain.Entities;

public class Conception
{
    public Guid Id { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [ForeignKey("FatherId")]
    public virtual Cattle Father { get; set; } = null!;
    public Guid FatherId { get; set; }

    [ForeignKey("MotherId")]
    public virtual Cattle Mother { get; set; } = null!;
    public Guid MotherId { get; set; }
}