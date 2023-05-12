namespace CattleManager.Application.Application.Common.Interfaces.InCommon;

public class DataInMonth<T> where T : struct
{
    public string Month { get; set; } = null!;
    public T Value { get; set; }
}