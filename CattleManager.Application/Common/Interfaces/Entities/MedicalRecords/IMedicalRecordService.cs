using CattleManager.Application.Application.Common.Interfaces.InCommon;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public interface IMedicalRecordService
{
    Task<MedicalRecordResponse> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId);
    Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsFromCattleAsync(Guid cattleId, Guid userId);
    Task<AmountOfMedicalRecords> GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(Guid userId, int month, int year);
    Task<IEnumerable<DataInMonth<decimal>>> GetAmountOfMedicalRecordsLastMonthsAsync(Guid userId, int previousMonths);
    Task<MedicalRecordResponse> CreateMedicalRecordAsync(CreateMedicalRecord createMedicalRecord, Guid userId);
    Task<MedicalRecordResponse> EditMedicalRecordAsync(EditMedicalRecord editMedicalRecord, Guid userId, Guid routeId);
    Task DeleteMedicalRecordAsync(Guid medicalRecordId, Guid userId);
}