namespace CattleManager.Application.Domain.Entities;

public class CattleOwner
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}