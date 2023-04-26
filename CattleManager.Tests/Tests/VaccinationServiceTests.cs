using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccines;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using CattleManager.Tests.Providers;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class VaccinationServiceTests
{
    private readonly IVaccinationService _sut;
    private readonly IVaccinationRepository _vaccinationRepositoryMock;
    private readonly IVaccineRepository _vaccineRepositoryMock;
    private readonly ICattleRepository _cattleRepositoryMock;
    private readonly IGuidProvider _guidProvider;
    private readonly IMapper _mapperMock;
    private static readonly Guid _vaccinationId = Guid.NewGuid();
    private static readonly Guid _vaccineId = Guid.NewGuid();
    private static readonly Guid _cattleId = Guid.NewGuid();
    private static readonly Guid _userId = Guid.NewGuid();

    public VaccinationServiceTests()
    {
        _cattleRepositoryMock = A.Fake<ICattleRepository>();
        _vaccinationRepositoryMock = A.Fake<IVaccinationRepository>();
        _vaccineRepositoryMock = A.Fake<IVaccineRepository>();
        _mapperMock = A.Fake<IMapper>();
        _sut = new VaccinationService(
            _vaccinationRepositoryMock,
            _vaccineRepositoryMock,
            _cattleRepositoryMock,
            _mapperMock);
        _guidProvider = new GuidProvider();
    }

    [Fact]
    public async Task Get_Vaccination_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Vaccination? nullVaccination = null;
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(nullVaccination);

        async Task result() => await _sut.GetVaccinationByIdAsync(_vaccinationId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacinação com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Vaccination_By_Existent_Id_Returns_Vaccination()
    {
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(new Vaccination());
        Vaccination vaccination = new()
        {
            Id = _vaccinationId,
            DosageInMl = dosageInMl,
            CattleId = _cattleId,
            VaccineId = _vaccineId
        };
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(_cattleId, _vaccineId, dosageInMl, date, _vaccinationId);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(vaccination);

        VaccinationResponse vaccinationResponse = await _sut.GetVaccinationByIdAsync(_vaccinationId);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationResponse);
    }

    [Fact]
    public async Task Get_All_Vaccinations_From_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, false)).Returns(nullCattle);

        async Task result() => await _sut.GetAllVaccinationsFromCattleAsync(_cattleId, _userId, 1);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_All_Vaccinations_From_Cattle_With_Zero_Vaccinations_Returns_Empty_List()
    {
        A.CallTo(() => _vaccinationRepositoryMock.GetAmountOfPages(_cattleId, _userId)).Returns(1);
        A.CallTo(() => _vaccinationRepositoryMock.GetAllVaccinationsFromCattle(_cattleId, _userId, 1)).Returns(new List<Vaccination>());
        A.CallTo(() => _mapperMock.Map<List<VaccinationResponse>>(new List<Vaccination>())).Returns(new List<VaccinationResponse>());
        PaginatedVaccinationResponse expectedVaccinationResponse = new(new List<VaccinationResponse>(), 1, 1);

        var vaccinationsFromCattle = await _sut.GetAllVaccinationsFromCattleAsync(_cattleId, _userId, 1);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationsFromCattle);
    }

    [Fact]
    public async Task Get_All_Vaccinations_From_Cattle_Returns_Its_Vaccinations()
    {
        List<Vaccination> vaccinations = new()
        {
            new Vaccination() {Id = Guid.NewGuid(), VaccineId = Guid.NewGuid(), CattleId = Guid.NewGuid(), DosageInMl = 10},
        };
        List<VaccinationResponse> expectedVaccinationResponses = GenerateListOfVaccinationResponse(vaccinations);
        A.CallTo(() => _vaccinationRepositoryMock.GetAmountOfPages(_cattleId, _userId)).Returns(1);
        A.CallTo(() => _vaccinationRepositoryMock.GetAllVaccinationsFromCattle(_cattleId, _userId, 1)).Returns(vaccinations);
        A.CallTo(() => _mapperMock.Map<List<VaccinationResponse>>(vaccinations)).Returns(expectedVaccinationResponses);
        PaginatedVaccinationResponse expectedResponse = new(expectedVaccinationResponses, 1, 1);

        PaginatedVaccinationResponse vaccinationResponses = await _sut.GetAllVaccinationsFromCattleAsync(_cattleId, _userId, 1);

        Assert.Equivalent(expectedResponse, vaccinationResponses);
    }

    [Fact]
    public async Task Create_Vaccination_With_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Cattle? nullCattle = null;
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(_cattleId, _vaccineId, 10, DateOnly.FromDateTime(DateTime.Now));
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, true)).Returns(nullCattle);

        async Task result() => await _sut.CreateVaccinationAsync(vaccinationRequest, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Vaccination_With_Non_Existent_Vaccine_Throws_NotFoundException()
    {
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(_cattleId, _vaccineId, 10, DateOnly.FromDateTime(DateTime.Now));
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, true)).Returns(new Cattle());
        Vaccine? nullVaccine = null;
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(_vaccineId)).Returns(nullVaccine);

        async Task result() => await _sut.CreateVaccinationAsync(vaccinationRequest, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacina com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Vaccination_With_Valid_Data_Returns_Created_Vaccination()
    {
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(_cattleId, _vaccineId, dosageInMl, date);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(_cattleId, _userId, true)).Returns(new Cattle());
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(_vaccineId)).Returns(new Vaccine());
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(_cattleId, _vaccineId, dosageInMl, date, _vaccinationId);
        Vaccination vaccination = GenerateVaccinationFromCreateVaccinationRequest(_vaccinationId, vaccinationRequest);
        A.CallTo(() => _mapperMock.Map<Vaccination>(vaccinationRequest)).Returns(vaccination);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);

        VaccinationResponse vaccinationResponse = await _sut.CreateVaccinationAsync(vaccinationRequest, _userId);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationResponse);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Different_Route_Id_Than_Route_In_Request_Body_Throws_BadRequestException()
    {
        Guid routeId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(_vaccinationId, _cattleId, _vaccineId, dosageInMl, date);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, routeId, _userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Rota não coincide com o id da vacinação.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Vaccination_Throws_NotFoundException()
    {
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(_vaccinationId, _cattleId, _vaccineId, dosageInMl, date);
        Vaccination? nullVaccination = null;
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(nullVaccination);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, _vaccinationId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacinação com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Cattle_Throws_NotFoundException()
    {
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(_vaccinationId, _cattleId, _vaccineId, dosageInMl, date);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(new Vaccination());
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(_cattleId)).Returns(nullCattle);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, _vaccinationId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Vaccine_Throws_NotFoundException()
    {
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(_vaccinationId, _cattleId, _vaccineId, dosageInMl, date);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(new Vaccination());
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(_cattleId)).Returns(new Cattle());
        Vaccine? nullAppliedVaccine = null;
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(_vaccineId)).Returns(nullAppliedVaccine);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, _vaccinationId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacina com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Valid_Data_Returns_Edited_Vaccination()
    {
        // Arrange
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(_vaccinationId, _cattleId, _vaccineId, dosageInMl, date);

        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(_vaccinationId)).Returns(new Vaccination());
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(_cattleId)).Returns(new Cattle());
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(_vaccineId)).Returns(new Vaccine());

        Vaccination vaccination = GenerateVaccinationFromEditVaccinationRequest(_vaccinationId, vaccinationRequest);
        A.CallTo(() => _mapperMock.Map<Vaccination>(vaccinationRequest)).Returns(vaccination);
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(_cattleId, _vaccineId, dosageInMl, date, _vaccinationId);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);

        // Act
        VaccinationResponse vaccinationResponse = await _sut.EditVaccinationAsync(vaccinationRequest, _vaccinationId, _userId);

        // Assert
        Assert.Equivalent(expectedVaccinationResponse, vaccinationResponse);
    }

    private static List<VaccinationResponse> GenerateListOfVaccinationResponse(List<Vaccination> vaccinations)
    {
        List<VaccinationResponse> vaccinationResponses = new();
        foreach (Vaccination vaccination in vaccinations)
            vaccinationResponses.Add(new VaccinationResponse(vaccination.Id, vaccination.CattleId, vaccination.VaccineId, vaccination.DosageInMl, vaccination.Date));

        return vaccinationResponses;
    }

    private static CreateVaccinationRequest GenerateVaccinationRequest(Guid cattleId, Guid vaccineId, decimal dosageInMl, DateOnly date)
    {
        return new CreateVaccinationRequest(cattleId, vaccineId, dosageInMl, date);
    }

    private static Vaccination GenerateVaccinationFromCreateVaccinationRequest(Guid vaccinationId, CreateVaccinationRequest vaccinationRequest)
    {
        return new Vaccination()
        {
            Id = vaccinationId,
            DosageInMl = vaccinationRequest.DosageInMl,
            Date = vaccinationRequest.Date,
            CattleId = vaccinationRequest.CattleId,
            VaccineId = vaccinationRequest.VaccineId
        };
    }

    private VaccinationResponse GenerateVaccinationResponse(Guid cattleId, Guid vaccineId, decimal dosageInMl, DateOnly date, Guid? vaccinationId)
    {
        Guid vaccinationResponseId = _guidProvider.NewGuid();
        return new VaccinationResponse(vaccinationId ?? vaccinationResponseId, cattleId, vaccineId, dosageInMl, date);
    }

    private static EditVaccinationRequest GenerateEditVaccinationRequest(Guid vaccinationId, Guid cattleId, Guid vaccineId, decimal dosageInMl, DateOnly date)
    {
        return new EditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);
    }

    private static Vaccination GenerateVaccinationFromEditVaccinationRequest(Guid vaccinationId, EditVaccinationRequest vaccinationRequest)
    {
        return new Vaccination()
        {
            Id = vaccinationId,
            DosageInMl = vaccinationRequest.DosageInMl,
            Date = vaccinationRequest.Date,
            CattleId = vaccinationRequest.CattleId,
            VaccineId = vaccinationRequest.VaccineId
        };
    }
}