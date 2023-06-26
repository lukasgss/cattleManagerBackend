namespace CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;

public interface IDateTimeProvider
{
    DateTime Now();
    DateTime UtcNow();
}