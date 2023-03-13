namespace CattleManager.Application.Domain.Entities;

public class Vaccine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public ICollection<Cattle> Cattle { get; set; } = null!;
}