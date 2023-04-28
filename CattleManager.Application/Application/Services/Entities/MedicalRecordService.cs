using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly ICattleRepository _cattleRepository;
    private readonly IMapper _mapper;
    private readonly IGuidProvider _guidProvider;

    public MedicalRecordService(
        IMedicalRecordRepository medicalRecordRepository,
        ICattleRepository cattleRepository,
        IMapper mapper,
        IGuidProvider guidProvider)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _cattleRepository = cattleRepository;
        _mapper = mapper;
        _guidProvider = guidProvider;
    }

    public async Task<MedicalRecordResponse> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId)
    {
        MedicalRecord? medicalRecord = await _medicalRecordRepository.GetMedicalRecordByIdAsync(medicalRecordId, userId);
        if (medicalRecord is null)
            throw new NotFoundException("Registro médico com o id especificado não existe.");

        return new MedicalRecordResponse()
        {
            Id = medicalRecord.Id,
            Description = medicalRecord.Description,
            Date = medicalRecord.Date,
            Location = medicalRecord.Location,
            Type = medicalRecord.Type,
            CattleId = medicalRecord.CattleId
        };
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsFromCattleAsync(Guid cattleId, Guid userId)
    {
        Cattle? cattle = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        IEnumerable<MedicalRecord> medicalRecords = await _medicalRecordRepository.GetAllMedicalRecordsFromCattleAsync(cattleId, userId);
        return _mapper.Map<List<MedicalRecordResponse>>(medicalRecords);
    }

    public async Task<MedicalRecordResponse> CreateMedicalRecordAsync(CreateMedicalRecord createMedicalRecord, Guid userId)
    {
        Cattle? cattle = await _cattleRepository.GetCattleById(createMedicalRecord.CattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        MedicalRecord medicalRecord = new()
        {
            Id = _guidProvider.NewGuid(),
            Date = createMedicalRecord.Date,
            Description = createMedicalRecord.Description,
            Location = createMedicalRecord.Location,
            Type = createMedicalRecord.Type,
            CattleId = cattle.Id
        };
        _medicalRecordRepository.Add(medicalRecord);
        await _medicalRecordRepository.CommitAsync();

        return new MedicalRecordResponse()
        {
            Id = medicalRecord.Id,
            Date = medicalRecord.Date,
            Description = medicalRecord.Description,
            Location = medicalRecord.Location,
            Type = medicalRecord.Type,
            CattleId = medicalRecord.CattleId
        };
    }
}