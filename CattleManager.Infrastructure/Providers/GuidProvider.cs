using CattleManager.Application.Application.Common.Interfaces.GuidProvider;

namespace CattleManager.Application.Infrastructure.Providers;

public class GuidProvider : IGuidProvider
{
    public Guid NewGuid() => Guid.NewGuid();
}