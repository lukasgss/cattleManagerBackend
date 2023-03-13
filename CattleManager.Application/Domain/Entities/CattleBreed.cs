namespace CattleManager.Application.Domain.Entities;

public class CattleBreed
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal QuantityInPercentage { get; set; }

    public Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
    public Breed Breed { get; set; } = null!;
    public Guid BreedId { get; set; }
}