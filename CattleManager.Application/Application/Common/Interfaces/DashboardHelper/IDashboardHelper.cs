using CattleManager.Application.Application.Common.Interfaces.Dashboard;
using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Interfaces.DashboardHelper;

public interface IDashboardHelper
{
    IEnumerable<DataInMonth<decimal>> FillTotalSumOfValueByMonths<T>(IEnumerable<IEnumerable<T>> dataToFill, int previousMonths) where T : IDataByMonth;
    IEnumerable<DataInMonth<decimal>> FillTotalCountOfEntityByMonths<T>(IEnumerable<IEnumerable<T>> dataToFill, int previousMonths) where T : IDataByMonth;
}