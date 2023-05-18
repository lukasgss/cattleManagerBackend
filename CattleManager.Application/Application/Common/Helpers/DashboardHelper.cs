using System.Globalization;
using CattleManager.Application.Application.Common.Interfaces.Dashboard;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Helpers;

public class DashboardHelper : IDashboardHelper
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public DashboardHelper(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public IEnumerable<DataInMonth<decimal>> FillTotalCountOfEntityByMonths<T>(
        IEnumerable<IEnumerable<T>> dataToFill, int previousMonths) where T : IDataByMonth
    {
        List<DataInMonth<decimal>> dataInMonths = new();

        foreach (var currentData in dataToFill)
        {
            T? data = currentData.FirstOrDefault();
            if (data is null)
                break;

            DataInMonth<decimal> dataInMonth = new()
            {
                Month = data.Date.ToString("MMM", new CultureInfo("pt-BR")),
                Value = currentData.Count()
            };
            dataInMonths.Add(dataInMonth);
        }

        // Adds 1 because the current month is exclusive
        // ex.: previousMonths value is 4, it returns the current month and data from the 4 previouses 
        if (dataInMonths.Count != previousMonths + 1)
            return FillEmptyMonthsWithZeroValue(dataInMonths, previousMonths);

        return dataInMonths;
    }

    public IEnumerable<DataInMonth<decimal>> FillTotalSumOfValueByMonths<T>(
        IEnumerable<IEnumerable<T>> dataToFill, int previousMonths) where T : IDataByMonth
    {
        List<DataInMonth<decimal>> dataInMonths = new();

        foreach (var currentData in dataToFill)
        {
            T? data = currentData.FirstOrDefault();
            if (data is null)
                break;

            DataInMonth<decimal> dataInMonth = new()
            {
                Month = data.Date.ToString("MMM", new CultureInfo("pt-BR")),
                Value = currentData.Sum(x => x.Value)
            };
            dataInMonths.Add(dataInMonth);
        }

        // Adds 1 because the current month is exclusive
        // ex.: previousMonths value is 4, it returns the current month and data from the 4 previouses 
        if (dataInMonths.Count != previousMonths + 1)
            return FillEmptyMonthsWithZeroValue(dataInMonths, previousMonths);

        return dataInMonths;
    }

    private DataInMonth<decimal>[] FillEmptyMonthsWithZeroValue(List<DataInMonth<decimal>> dataInMonths, int previousMonths)
    {
        DataInMonth<decimal>[] formattedDataInMonths = new DataInMonth<decimal>[previousMonths + 1];
        DateOnly currentDate = DateOnly.FromDateTime(_dateTimeProvider.Now().AddMonths(-previousMonths));

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

            formattedDataInMonths[i] = new()
            {
                Month = monthName,
                Value = 0
            };

            currentDate = currentDate.AddMonths(1);
        }

        return formattedDataInMonths;
    }
}