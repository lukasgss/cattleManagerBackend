namespace CattleManager.Application.Domain.Entities;

public class MilkProduction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal MilkInLiters { get; set; }
    public string PeriodOfDay { get; set; } = null!;
    public DateOnly Date { get; set; }

    public Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
}