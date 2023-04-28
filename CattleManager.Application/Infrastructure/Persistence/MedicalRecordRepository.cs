using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Infrastructure.Persistence.DataContext;

namespace CattleManager.Application.Infrastructure.Persistence;

public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    private readonly AppDbContext _dbContext;
    public MedicalRecordRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MedicalRecord>> GetAllMedicalRecordsFromCattleAsync(Guid cattleId, Guid userId)
    {
        return await _dbContext.MedicalRecords
            .AsNoTracking()
            .Where(record => record.Cattle.Id == cattleId
                && record.Cattle.Users.Any(user => user.Id == userId))
            .OrderByDescending(record => record.Date)
            .ToListAsync();
    }

    public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId)
    {
        return await _dbContext.MedicalRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(record => record.Id == medicalRecordId
                && record.Cattle.Users.Any(user => user.Id == userId));
    }
}