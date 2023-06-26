using System.ComponentModel.DataAnnotations;

namespace CattleManager.Application.Domain.Entities;

public class Breed
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Cattle> Cattle { get; set; } = null!;
    public virtual ICollection<CattleBreed> CattleBreeds { get; set; } = null!;
}