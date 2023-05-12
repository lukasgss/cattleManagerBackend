using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CattleManager.Application.Application.Common.Helpers;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class DashboardHelperTests
{
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly IDashboardHelper _sut;
    public DashboardHelperTests()
    {
        _dateTimeProviderMock = A.Fake<IDateTimeProvider>();
        _sut = new DashboardHelper(_dateTimeProviderMock);
    }

    [Fact]
    public void Fills_Empty_Months_With_Zero_Value_Correctly()
    {
        const int previousMonths = 4;
        DateTime currentDateTime = new(2020, 1, 1);
        DateOnly currentDateOnly = DateOnly.FromDateTime(currentDateTime);
        List<DataInMonth<decimal>> dataInMonths = GenerateDataInMonthData();
        A.CallTo(() => _dateTimeProviderMock.Now()).Returns(currentDateTime);
        DataInMonth<decimal>[] expectedDataInMonths = GenerateDataInMonthWithEmptyValuesAsZero(previousMonths, dataInMonths, currentDateOnly);

        DataInMonth<decimal>[] dataInMonthsResult = _sut.FillEmptyMonthsWithZeroValue(dataInMonths, previousMonths);

        Assert.Equivalent(expectedDataInMonths, dataInMonthsResult);
    }

    private static DataInMonth<decimal>[] GenerateDataInMonthWithEmptyValuesAsZero(int previousMonths, List<DataInMonth<decimal>> dataInMonths, DateOnly dateToday)
    {
        DataInMonth<decimal>[] formattedDataInMonths = new DataInMonth<decimal>[previousMonths + 1];
        DateOnly currentDate = dateToday.AddMonths(-previousMonths);
        for (int i = 0; i <= previousMonths; i++)
        {
            string monthName = currentDate.ToString("MMM", new CultureInfo("pt-BR"));
            int index = dataInMonths.FindIndex(x => x.Month == monthName);
            if (index != -1)
            {
                formattedDataInMonths[i] = dataInMonths[index];
                currentDate = currentDate.AddMonths(1);
                continue;
            }

            DataInMonth<decimal> dataInMonth = new()
            {
                Month = monthName,
                Value = 0
            };

            formattedDataInMonths[i] = dataInMonth;
            currentDate = currentDate.AddMonths(1);
        }
        return formattedDataInMonths;
    }

    private static List<DataInMonth<decimal>> GenerateDataInMonthData()
    {
        return new List<DataInMonth<decimal>>()
        {
            new DataInMonth<decimal>()
            {
                Month = "jan.",
                Value = 10
            },
            new DataInMonth<decimal>()
            {
                Month = "fev.",
                Value = 20
            }
        };
    }
}