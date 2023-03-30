using CattleManager.Application.Domain.Entities;

namespace CatetleManager.Application.Domain.Entities;

public class Conception
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }

    public Guid FatherId { get; set; }
    public virtual Cattle Father { get; set; } = null!;
    public Guid MotherId { get; set; }
    public virtual Cattle Mother { get; set; } = null!;
}