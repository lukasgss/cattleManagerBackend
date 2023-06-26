using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class CattleBreed
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column(TypeName = "decimal(11, 10)")]
    public decimal QuantityInPercentage { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }

    [ForeignKey("BreedId")]
    public virtual Breed Breed { get; set; } = null!;
    public Guid BreedId { get; set; }
}