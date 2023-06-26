using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;

namespace CattleManager.Application.Infrastructure.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => DateTime.Now;
    public DateTime UtcNow() => DateTime.UtcNow;
}