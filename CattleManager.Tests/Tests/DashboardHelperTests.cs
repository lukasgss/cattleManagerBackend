using System;
using System.Collections.Generic;
using CattleManager.Application.Application.Common.Helpers;
using CattleManager.Application.Application.Common.Interfaces.Dashboard;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Tests.Tests.TestTypes;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class DashboardHelperTests
{
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly IDashboardHelper _sut;
    private readonly DateTime _currentDateTime = new(2023, 1, 1);
    private const int _previousMonths = 1;
    private const int _value = 2;
    public DashboardHelperTests()
    {
        _dateTimeProviderMock = A.Fake<IDateTimeProvider>();
        _sut = new DashboardHelper(_dateTimeProviderMock);
    }

    [Fact]
    public void Fills_Total_Count_Of_Entity_By_Months()
    {
        List<List<IDataByMonth>> dataByMonths = GenerateDataByMonthList();
        A.CallTo(() => _dateTimeProviderMock.Now()).Returns(_currentDateTime);
        DataInMonth<decimal>[] expectedData = GenerateExpectedDataInMonthResponse(dataByMonths);

        IEnumerable<DataInMonth<decimal>> data = _sut.FillTotalCountOfEntityByMonths(dataByMonths, _previousMonths);

        Assert.Equivalent(expectedData, data);
    }

    [Fact]
    public void Fills_Total_Sum_Of_Value_By_Months()
    {
        List<List<IDataByMonth>> dataByMonths = GenerateDataByMonthList();
        A.CallTo(() => _dateTimeProviderMock.Now()).Returns(_currentDateTime);
        DataInMonth<decimal>[] expectedData = GenerateExpectedDataInMonthResponse(dataByMonths);

        IEnumerable<DataInMonth<decimal>> data = _sut.FillTotalCountOfEntityByMonths(dataByMonths, _previousMonths);

        Assert.Equivalent(expectedData, data);
    }

    private static DataInMonth<decimal>[] GenerateExpectedDataInMonthResponse(List<List<IDataByMonth>> dataByMonths)
    {
        return new DataInMonth<decimal>[]
        {
            new DataInMonth<decimal>()
            {
                Month = "nov.",
                Value = dataByMonths[0].Count,
            },
            new DataInMonth<decimal>()
            {
                Month = "dez.",
                Value = dataByMonths[1].Count,
            },
        };
    }

    private List<List<IDataByMonth>> GenerateDataByMonthList()
    {
        return new()
        {
            // november
            new List<IDataByMonth>()
            {
                new DataByMonthTest()
                {
                    Date = DateOnly.FromDateTime(_currentDateTime).AddMonths(-2),
                    Value = 0,
                },
                new DataByMonthTest()
                {
                    Date = DateOnly.FromDateTime(_currentDateTime),
                    Value = _value
                },
            },
            // december
            new List<IDataByMonth>()
            {
                new DataByMonthTest()
                {
                    Date = DateOnly.FromDateTime(_currentDateTime).AddMonths(-1),
                    Value = 0,
                },
            }
        };
    }
}