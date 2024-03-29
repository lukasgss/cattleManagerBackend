using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class MedicalRecordServiceTests
{
    private readonly IMedicalRecordService _sut;
    private readonly IMedicalRecordRepository _medicalRecordRepositoryMock;
    private readonly IServiceValidations _serviceValidationsMock;
    private readonly IMapper _mapperMock;
    private readonly ICattleRepository _cattleRepositoryMock;
    private readonly IDashboardHelper _dashboardHelperMock;
    private static readonly Guid _medicalRecordId = Guid.NewGuid();
    private static readonly Guid _userId = Guid.NewGuid();
    private static readonly Guid _cattleId = Guid.NewGuid();
    private static readonly DateOnly _medicalRecordDate = new(2020, 8, 8);

    public MedicalRecordServiceTests()
    {
        _medicalRecordRepositoryMock = A.Fake<IMedicalRecordRepository>();
        _cattleRepositoryMock = A.Fake<ICattleRepository>();
        _serviceValidationsMock = A.Fake<IServiceValidations>();
        _dashboardHelperMock = A.Fake<IDashboardHelper>();
        _mapperMock = A.Fake<IMapper>();
        _sut = new MedicalRecordService(
            _medicalRecordRepositoryMock,
            _cattleRepositoryMock,
            _serviceValidationsMock,
            _dashboardHelperMock,
            _mapperMock);
    }

    [Fact]
    public async Task Get_Medical_Record_By_Non_Existent_Id_Throws_NotFoundException()
    {
        MedicalRecord? nullMedicalRecord = null;
        A.CallTo(() => _medicalRecordRepositoryMock.GetMedicalRecordByIdAsync(_medicalRecordId, _userId, false)).Returns(nullMedicalRecord);

        async Task result() => await _sut.GetMedicalRecordByIdAsync(_medicalRecordId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Registro médico com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Medical_Record_By_Id_Returns_Medical_Record()
    {
        MedicalRecord medicalRecord = GenerateMedicalRecord();
        A.CallTo(() => _medicalRecordRepositoryMock.GetMedicalRecordByIdAsync(_medicalRecordId, _userId, false)).Returns(medicalRecord);
        MedicalRecordResponse expectedMedicalRecordResponse = GenerateMedicalRecordResponseFromMedicalRecord(medicalRecord);

        MedicalRecordResponse medicalRecordResponse = await _sut.GetMedicalRecordByIdAsync(_medicalRecordId, _userId);

        Assert.Equivalent(expectedMedicalRecordResponse, medicalRecordResponse);
    }

    [Fact]
    public async Task Get_All_Medical_Records_From_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, false)).Returns(nullCattle);

        async Task result() => await _sut.GetAllMedicalRecordsFromCattleAsync(_cattleId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_All_Medical_Records_From_Cattle_Returns_All_Medical_Records()
    {
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, false)).Returns(new Cattle());
        MedicalRecord medicalRecord = GenerateMedicalRecord();
        List<MedicalRecord> medicalRecords = new() { medicalRecord };
        A.CallTo(() => _medicalRecordRepositoryMock.GetAllMedicalRecordsFromCattleAsync(_cattleId, _userId)).Returns(medicalRecords);
        List<MedicalRecordResponse> expectedMedicalRecordsResponse = new() { GenerateMedicalRecordResponseFromMedicalRecord(medicalRecord) };
        A.CallTo(() => _mapperMock.Map<List<MedicalRecordResponse>>(medicalRecords)).Returns(expectedMedicalRecordsResponse);

        IEnumerable<MedicalRecordResponse> medicalRecordsResponse = await _sut.GetAllMedicalRecordsFromCattleAsync(_cattleId, _userId);

        Assert.Equivalent(expectedMedicalRecordsResponse, medicalRecordsResponse);
    }

    [Fact]
    public async Task Get_Amount_Of_Medical_Records_Returns_Amount_Of_Medical_Records()
    {
        const int month = 8;
        const int year = 2019;
        AmountOfMedicalRecords expectedAmountOfMedicalRecords = GenerateAmountOfMedicalRecords();
        A.CallTo(() => _medicalRecordRepositoryMock.GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(_userId, month, year))
            .Returns(expectedAmountOfMedicalRecords);

        AmountOfMedicalRecords amountOfMedicalRecords = await _sut.GetAmountOfMedicalRecordsInSpecificMonthAndYearAsync(_userId, month, year);

        Assert.Equivalent(expectedAmountOfMedicalRecords, amountOfMedicalRecords);
    }

    [Fact]
    public async Task Create_Medical_Record_With_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Cattle? nullCattle = null;
        CreateMedicalRecord createMedicalRecord = GenerateCreateMedicalRecord();
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, false)).Returns(nullCattle);

        async Task result() => await _sut.CreateMedicalRecordAsync(createMedicalRecord, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Medical_Record_Returns_Created_Medical_Record()
    {
        CreateMedicalRecord createMedicalRecord = GenerateCreateMedicalRecord();
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, false)).Returns(new Cattle() { Id = _cattleId });
        MedicalRecord medicalRecord = GenerateMedicalRecord();
        A.CallTo(() => _mapperMock.Map<MedicalRecord>(createMedicalRecord)).Returns(medicalRecord);
        MedicalRecordResponse expectedMedicalRecordResponse = GenerateMedicalRecordResponseFromMedicalRecord(medicalRecord);
        A.CallTo(() => _mapperMock.Map<MedicalRecordResponse>(medicalRecord)).Returns(expectedMedicalRecordResponse);

        MedicalRecordResponse medicalRecordResponse = await _sut.CreateMedicalRecordAsync(createMedicalRecord, _userId);

        Assert.Equivalent(expectedMedicalRecordResponse, medicalRecordResponse);
    }

    [Fact]
    public async Task Edit_Medical_Record_With_Different_Route_Id_From_Specified_In_Request_Throws_BadRequestException()
    {
        Guid routeId = Guid.NewGuid();
        EditMedicalRecord editMedicalRecord = GenerateEditMedicalRecord();

        async Task result() => await _sut.EditMedicalRecordAsync(editMedicalRecord, _userId, routeId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Rota não coincide com o id especificado.", exception.Message);
    }

    [Fact]
    public async Task Edit_Non_Existent_Medical_Record_Throws_NotFoundException()
    {
        MedicalRecord? nullMedicalRecord = null;
        EditMedicalRecord editMedicalRecord = GenerateEditMedicalRecord();
        A.CallTo(() => _medicalRecordRepositoryMock.GetMedicalRecordByIdAsync(_medicalRecordId, _userId, false)).Returns(nullMedicalRecord);

        async Task result() => await _sut.EditMedicalRecordAsync(editMedicalRecord, _userId, _medicalRecordId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Registro médico com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Medical_Record_Returns_Edited_Medical_Record()
    {
        EditMedicalRecord editMedicalRecord = GenerateEditMedicalRecord();
        MedicalRecord medicalRecord = GenerateMedicalRecord();
        A.CallTo(() => _medicalRecordRepositoryMock.GetMedicalRecordByIdAsync(_medicalRecordId, _userId, false)).Returns(medicalRecord);
        A.CallTo(() => _mapperMock.Map<MedicalRecord>(editMedicalRecord)).Returns(medicalRecord);
        MedicalRecordResponse expectedMedicalRecordResponse = GenerateMedicalRecordResponseFromMedicalRecord(medicalRecord);
        A.CallTo(() => _mapperMock.Map<MedicalRecordResponse>(medicalRecord)).Returns(expectedMedicalRecordResponse);

        MedicalRecordResponse medicalRecordResponse = await _sut.EditMedicalRecordAsync(editMedicalRecord, _userId, _medicalRecordId);

        Assert.Equivalent(expectedMedicalRecordResponse, medicalRecordResponse);
    }

    [Fact]
    public async Task Delete_Non_Existent_Medical_Record_Throws_NotFoundException()
    {
        MedicalRecord? nullMedicalRecord = null;
        A.CallTo(() => _medicalRecordRepositoryMock.GetMedicalRecordByIdAsync(_medicalRecordId, _userId, true)).Returns(nullMedicalRecord);

        async Task result() => await _sut.DeleteMedicalRecordAsync(_medicalRecordId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Registro médico com o id especificado não existe.", exception.Message);
    }

    private static AmountOfMedicalRecords GenerateAmountOfMedicalRecords()
    {
        return new AmountOfMedicalRecords()
        {
            Amount = 8
        };
    }

    private static EditMedicalRecord GenerateEditMedicalRecord()
    {
        return new EditMedicalRecord(
            Id: _medicalRecordId,
            Description: "description",
            Date: _medicalRecordDate,
            Type: 0,
            Location: "location",
            CattleId: _cattleId);
    }

    private static CreateMedicalRecord GenerateCreateMedicalRecord()
    {
        return new CreateMedicalRecord(
            Description: "description",
            Date: _medicalRecordDate,
            Type: 0,
            Location: "location",
            CattleId: _cattleId);
    }

    private static MedicalRecordResponse GenerateMedicalRecordResponseFromMedicalRecord(MedicalRecord medicalRecord)
    {
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

    private static MedicalRecord GenerateMedicalRecord()
    {
        return new MedicalRecord()
        {
            Id = _medicalRecordId,
            Date = _medicalRecordDate,
            Description = "description",
            Type = 0,
            Location = "location",
            CattleId = _cattleId,
        };
    }
}