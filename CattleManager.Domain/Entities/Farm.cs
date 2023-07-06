using System.ComponentModel.DataAnnotations;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Domain.Entities;

public class Farm
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = null!;

    public virtual ICollection<FarmOwner> Owners { get; set; } = null!;
    public virtual ICollection<FarmMember> Members { get; set; } = null!;
}