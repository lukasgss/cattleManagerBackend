using CattleManager.Application.Application.Common.Interfaces.Dashboard;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public class MedicalRecordByMonth : IDataByMonth
{
    public DateOnly Date { get; set; }
    public decimal Value { get; set; }
}