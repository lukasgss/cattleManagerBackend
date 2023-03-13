namespace CattleManager.Application.Domain.Entities;

public class Breed
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public ICollection<Cattle> Cattle { get; set; } = null!;
    public ICollection<CattleBreed> CattleBreeds { get; set; } = null!;
}