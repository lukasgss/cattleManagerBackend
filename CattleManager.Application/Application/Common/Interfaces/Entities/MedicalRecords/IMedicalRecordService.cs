namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public interface IMedicalRecordService
{
    Task<MedicalRecordResponse> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId);
    Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsFromCattleAsync(Guid cattleId, Guid userId);
    Task<MedicalRecordResponse> CreateMedicalRecordAsync(CreateMedicalRecord createMedicalRecord, Guid userId);
}