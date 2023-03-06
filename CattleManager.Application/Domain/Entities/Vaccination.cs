namespace CattleManager.Application.Domain.Entities;

public class Vaccination
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal DosageInMl { get; set; }
    public DateOnly Date { get; set; }

    public Cattle Cattle { get; set; } = null!;
    public Guid CattleId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;
    public Guid VaccineId { get; set; }
}