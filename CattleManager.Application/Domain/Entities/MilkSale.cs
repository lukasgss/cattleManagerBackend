using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class MilkSale
{
    public Guid Id { get; set; }

    [Required, Column(TypeName = "decimal(7, 2)")]
    public decimal MilkInLiters { get; set; }

    [Required, Column(TypeName = "decimal(12, 2)")]
    public decimal PricePerLiter { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [ForeignKey("OwnerId")]
    public User Owner { get; set; } = null!;
}