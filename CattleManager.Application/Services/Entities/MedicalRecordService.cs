using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.CommonValidations;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly ICattleRepository _cattleRepository;
    private readonly IServiceValidations _serviceValidations;
    private readonly IDashboardHelper _dashboardHelper;
    private readonly IMapper _mapper;

    public MedicalRecordService(
        IMedicalRecordRepository medicalRecordRepository,
        ICattleRepository cattleRepository,
        IServiceValidations serviceValidations,
        IDashboardHelper dashboardHelper,
        IMapper mapper)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _cattleRepository = cattleRepository;
        _serviceValidations = serviceValidations;
        _dashboardHelper = dashboardHelper;
        _mapper = mapper;
    }

    public async Task<MedicalRecordResponse> GetMedicalRecordByIdAsync(Guid medicalRecordId, Guid userId)
    {
        MedicalRecord? medicalRecord = await _medicalRecordRepository.GetMedicalRecordByIdAsync(medicalRecordId, userId, false);
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

    public async Task<AmountOfMedicalRecords> GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(Guid userId, int month, int year)
    {
        ServiceValidations.ValidateMonth(month);
        _serviceValidations.ValidateDate(month, year);

        return await _medicalRecordRepository.GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(userId, month, year);
    }

    public async Task<IEnumerable<DataInMonth<decimal>>> GetAmountOfMedicalRecordsLastMonthsAsync(Guid userId, int previousMonths)
    {
        if (previousMonths <= 0)
            throw new BadRequestException("Valor dos meses anteriores deve ser maior ou igual a 1.");

        var medicalRecordLastMonths = await _medicalRecordRepository.GetAmountOfMedicalRecordsLastMonthsAsync(userId, previousMonths);
        return _dashboardHelper.FillTotalCountOfEntityByMonths(medicalRecordLastMonths, previousMonths);
    }

    public async Task<MedicalRecordResponse> CreateMedicalRecordAsync(CreateMedicalRecord createMedicalRecord, Guid userId)
    {
        Cattle? cattle = await _cattleRepository.GetCattleById(createMedicalRecord.CattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        MedicalRecord medicalRecord = _mapper.Map<MedicalRecord>(createMedicalRecord);
        _medicalRecordRepository.Add(medicalRecord);
        await _medicalRecordRepository.CommitAsync();

        return _mapper.Map<MedicalRecordResponse>(medicalRecord);
    }

    public async Task<MedicalRecordResponse> EditMedicalRecordAsync(EditMedicalRecord editMedicalRecord, Guid userId, Guid routeId)
    {
        if (editMedicalRecord.Id != routeId)
            throw new BadRequestException("Rota não coincide com o id especificado.");

        MedicalRecord? medicalRecordToEdit = await _medicalRecordRepository.GetMedicalRecordByIdAsync(editMedicalRecord.Id, userId, false);
        if (medicalRecordToEdit is null)
            throw new NotFoundException("Registro médico com o id especificado não existe.");

        MedicalRecord medicalRecord = _mapper.Map<MedicalRecord>(editMedicalRecord);
        _medicalRecordRepository.Update(medicalRecord);
        await _medicalRecordRepository.CommitAsync();

        return _mapper.Map<MedicalRecordResponse>(medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(Guid medicalRecordId, Guid userId)
    {
        MedicalRecord? medicalRecordToDelete = await _medicalRecordRepository.GetMedicalRecordByIdAsync(medicalRecordId, userId);
        if (medicalRecordToDelete is null)
            throw new NotFoundException("Registro médico com o id especificado não existe.");

        _medicalRecordRepository.Delete(medicalRecordToDelete);
        await _medicalRecordRepository.CommitAsync();
    }
}