using System.ComponentModel.DataAnnotations.Schema;

namespace CattleManager.Application.Domain.Entities;

public class CattleOwner
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    public Guid UserId { get; set; }

    [ForeignKey("CattleId")]
    public virtual Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}