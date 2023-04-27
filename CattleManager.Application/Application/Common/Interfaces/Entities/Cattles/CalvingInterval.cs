namespace CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;

public class CalvingInterval
{
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
    public int Years { get; set; }
    public int Months { get; init; }
    public int Days { get; init; }
}