using System;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;

namespace CattleManager.Tests.Providers;

public class GuidProvider : IGuidProvider
{
    public Guid NewGuid() => new("dddddddd-dddd-dddd-dddd-dddddddddddd");
}