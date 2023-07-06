using System.ComponentModel.DataAnnotations.Schema;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Domain.Entities;

[Table("FarmMembers")]
public class FarmMember
{
    [ForeignKey("FarmId")]
    public virtual Farm Farm { get; set; } = null!;
    public Guid FarmId { get; set; }

    [ForeignKey("MemberId")]
    public virtual User User { get; set; } = null!;
    public Guid MemberId { get; set; }
}