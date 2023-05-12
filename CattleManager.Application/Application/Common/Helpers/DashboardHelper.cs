using System.Globalization;
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

    public DataInMonth<decimal>[] FillEmptyMonthsWithZeroValue(List<DataInMonth<decimal>> dataInMonths, int previousMonths)
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