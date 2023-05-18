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

    public async Task<AmountOfMedicalRecords> GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(Guid userId, int month, int year)
    {
        DateOnly startDate = new(year, month, 1);
        DateOnly endDate = startDate.AddDays(DateTime.DaysInMonth(year, month) - 1);
        int amountOfMedicalRecords = await _dbContext.MedicalRecords
            .AsNoTracking()
            .Where(record => record.Cattle.Users.Any(user => user.Id == userId)
                && record.Date >= startDate && record.Date <= endDate)
            .Select(record => record.Id).CountAsync();

        return new AmountOfMedicalRecords()
        {
            Amount = amountOfMedicalRecords
        };
    }

    public async Task<IEnumerable<IEnumerable<MedicalRecordByMonth>>> GetAmountOfMedicalRecordsLastMonthsAsync(Guid userId, int previousMonths)
    {
        DateOnly endDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly startDate = endDate.AddMonths(-previousMonths);

        var query = await _dbContext.MedicalRecords
            .Where(medicalRecord => medicalRecord.Cattle.Users.Any(user => user.Id == userId) && medicalRecord.Date >= startDate && medicalRecord.Date <= endDate)
            .Select(medicalRecord => new MedicalRecordByMonth()
            {
                Date = medicalRecord.Date,
            })
            .GroupBy(milkProduction => milkProduction.Date.Month)
            .ToListAsync();

        return query.ConvertAll(x => x.ToList());
    }

    public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId, bool trackChanges = true)
    {
        return trackChanges ? await _dbContext.MedicalRecords
            .FirstOrDefaultAsync(record => record.Id == medicalRecordId
                && record.Cattle.Users.Any(user => user.Id == userId))
            :
            await _dbContext.MedicalRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(record => record.Id == medicalRecordId
                && record.Cattle.Users.Any(user => user.Id == userId));
    }
}