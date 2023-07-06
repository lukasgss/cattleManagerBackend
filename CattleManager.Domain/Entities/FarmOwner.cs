using System.ComponentModel.DataAnnotations.Schema;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Domain.Entities;

[Table("FarmOwners")]
public class FarmOwner
{
    [ForeignKey("FarmId")]
    public virtual Farm Farm { get; set; } = null!;
    public Guid FarmId { get; set; }

    [ForeignKey("OwnerId")]
    public virtual User User { get; set; } = null!;
    public Guid OwnerId { get; set; }
}