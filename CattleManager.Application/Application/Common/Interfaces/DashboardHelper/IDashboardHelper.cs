using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Interfaces.DashboardHelper;

public interface IDashboardHelper
{
    DataInMonth<decimal>[] FillEmptyMonthsWithZeroValue(List<DataInMonth<decimal>> dataInMonths, int previousMonths);
}