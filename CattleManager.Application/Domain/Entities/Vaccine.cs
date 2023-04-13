using System.ComponentModel.DataAnnotations;

namespace CattleManager.Application.Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Vaccine
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [StringLength(255)]
    public string Name { get; set; } = null!;
    public virtual ICollection<Vaccination> Vaccinations { get; set; } = null!;
}