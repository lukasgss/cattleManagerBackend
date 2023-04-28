using CattleManager.Application.Application.Common.Interfaces.GenericRepository;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;

public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
{
    Task<MedicalRecord?> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId, bool trackChanges = true);
    Task<IEnumerable<MedicalRecord>> GetAllMedicalRecordsFromCattleAsync(Guid cattleId, Guid userId);
    Task<AmountOfMedicalRecords> GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(Guid userId, int month, int year);
}