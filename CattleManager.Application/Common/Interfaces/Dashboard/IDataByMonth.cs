namespace CattleManager.Application.Application.Common.Interfaces.Dashboard;

public interface IDataByMonth
{
    DateOnly Date { get; set; }
    decimal Value { get; set; }
}