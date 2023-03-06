namespace CattleManager.Application.Domain.Entities;

public class MilkProduction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal MilkPerDayInLiters { get; set; }
    public DateOnly Date { get; set; }

    public Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}