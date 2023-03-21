namespace CattleManager.Application.Domain.Entities;

public class Vaccine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public virtual ICollection<Vaccination> Vaccinations { get; set; } = null!;
}