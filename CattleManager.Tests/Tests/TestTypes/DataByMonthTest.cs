using System;
using CattleManager.Application.Application.Common.Interfaces.Dashboard;

namespace CattleManager.Tests.Tests.TestTypes;

public class DataByMonthTest : IDataByMonth
{
    public DateOnly Date { get; set; }
    public decimal Value { get; set; }
}