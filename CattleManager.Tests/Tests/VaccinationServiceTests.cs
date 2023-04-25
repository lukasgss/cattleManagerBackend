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
        Guid vaccinationId = Guid.NewGuid();
        Vaccination? nullVaccination = null;
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(nullVaccination);

        async Task result() => await _sut.GetVaccinationByIdAsync(vaccinationId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacinação com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Vaccination_By_Existent_Id_Returns_Vaccination()
    {
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        Guid vaccineId = Guid.NewGuid();
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(new Vaccination());
        Vaccination vaccination = new() { Id = vaccinationId, DosageInMl = dosageInMl, CattleId = cattleId, VaccineId = vaccineId };
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(cattleId, vaccineId, dosageInMl, date, vaccinationId);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(vaccination);

        VaccinationResponse vaccinationResponse = await _sut.GetVaccinationByIdAsync(vaccinationId);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationResponse);
    }

    [Fact]
    public async Task Get_All_Vaccinations_From_Cattle_With_Zero_Vaccinations_Returns_Empty_List()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _vaccinationRepositoryMock.GetAmountOfPages(cattleId, userId)).Returns(1);
        A.CallTo(() => _vaccinationRepositoryMock.GetAllVaccinationsFromCattle(cattleId, userId, 1)).Returns(new List<Vaccination>());
        A.CallTo(() => _mapperMock.Map<List<VaccinationResponse>>(new List<Vaccination>())).Returns(new List<VaccinationResponse>());
        PaginatedVaccinationResponse expectedVaccinationResponse = new(new List<VaccinationResponse>(), 1, 1);

        var vaccinationsFromCattle = await _sut.GetAllVaccinationsFromCattle(cattleId, userId, 1);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationsFromCattle);
    }

    [Fact]
    public async Task Get_All_Vaccinations_From_Cattle_Returns_Its_Vaccinations()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        List<Vaccination> vaccinations = new()
        {
            new Vaccination() {Id = Guid.NewGuid(), VaccineId = Guid.NewGuid(), CattleId = Guid.NewGuid(), DosageInMl = 10},
            new Vaccination() {Id = Guid.NewGuid(), VaccineId = Guid.NewGuid(), CattleId = Guid.NewGuid(), DosageInMl = 8},
            new Vaccination() {Id = Guid.NewGuid(), VaccineId = Guid.NewGuid(), CattleId = Guid.NewGuid(), DosageInMl = 6},
            new Vaccination() {Id = Guid.NewGuid(), VaccineId = Guid.NewGuid(), CattleId = Guid.NewGuid(), DosageInMl = 4},
        };
        List<VaccinationResponse> expectedVaccinationResponses = GenerateListOfVaccinationResponse(vaccinations);
        A.CallTo(() => _vaccinationRepositoryMock.GetAmountOfPages(cattleId, userId)).Returns(1);
        A.CallTo(() => _vaccinationRepositoryMock.GetAllVaccinationsFromCattle(cattleId, userId, 1)).Returns(vaccinations);
        A.CallTo(() => _mapperMock.Map<List<VaccinationResponse>>(vaccinations)).Returns(expectedVaccinationResponses);
        PaginatedVaccinationResponse expectedResponse = new(expectedVaccinationResponses, 1, 1);

        PaginatedVaccinationResponse vaccinationResponses = await _sut.GetAllVaccinationsFromCattle(cattleId, userId, 1);

        Assert.Equivalent(expectedResponse, vaccinationResponses);
    }

    [Fact]
    public async Task Create_Vaccination_With_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Cattle? nullCattle = null;
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(cattleId, vaccineId, 10, DateOnly.FromDateTime(DateTime.Now));
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(nullCattle);

        async Task result() => await _sut.CreateVaccinationAsync(vaccinationRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Vaccination_With_Non_Existent_Vaccine_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(cattleId, vaccineId, 10, DateOnly.FromDateTime(DateTime.Now));
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(new Cattle());
        Vaccine? nullVaccine = null;
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(vaccineId)).Returns(nullVaccine);

        async Task result() => await _sut.CreateVaccinationAsync(vaccinationRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacina com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Vaccination_With_Valid_Data_Returns_Created_Vaccination()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        CreateVaccinationRequest vaccinationRequest = GenerateVaccinationRequest(cattleId, vaccineId, dosageInMl, date);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(new Cattle());
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(vaccineId)).Returns(new Vaccine());
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(cattleId, vaccineId, dosageInMl, date, vaccinationId);
        Vaccination vaccination = GenerateVaccinationFromCreateVaccinationRequest(vaccinationId, vaccinationRequest);
        A.CallTo(() => _mapperMock.Map<Vaccination>(vaccinationRequest)).Returns(vaccination);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);

        VaccinationResponse vaccinationResponse = await _sut.CreateVaccinationAsync(vaccinationRequest, userId);

        Assert.Equivalent(expectedVaccinationResponse, vaccinationResponse);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Different_Route_Id_Than_Route_In_Request_Body_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid routeId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, routeId, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Rota não coincide com o id da vacinação.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Vaccination_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);
        Vaccination? nullVaccination = null;
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(nullVaccination);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, vaccinationId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacinação com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(new Vaccination());
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleId)).Returns(nullCattle);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, vaccinationId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Non_Existent_Vaccine_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(new Vaccination());
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleId)).Returns(new Cattle());
        Vaccine? nullAppliedVaccine = null;
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(vaccineId)).Returns(nullAppliedVaccine);

        async Task result() => await _sut.EditVaccinationAsync(vaccinationRequest, vaccinationId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Vacina com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Vaccination_With_Valid_Data_Returns_Edited_Vaccination()
    {
        Guid userId = Guid.NewGuid();
        Guid vaccinationId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid vaccineId = Guid.NewGuid();
        const decimal dosageInMl = 10;
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        EditVaccinationRequest vaccinationRequest = GenerateEditVaccinationRequest(vaccinationId, cattleId, vaccineId, dosageInMl, date);
        A.CallTo(() => _vaccinationRepositoryMock.GetVaccinationByIdAsync(vaccinationId)).Returns(new Vaccination());
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleId)).Returns(new Cattle());
        A.CallTo(() => _vaccineRepositoryMock.GetVaccineByIdAsync(vaccineId)).Returns(new Vaccine());
        Vaccination vaccination = GenerateVaccinationFromEditVaccinationRequest(vaccinationId, vaccinationRequest);
        A.CallTo(() => _mapperMock.Map<Vaccination>(vaccinationRequest)).Returns(vaccination);
        VaccinationResponse expectedVaccinationResponse = GenerateVaccinationResponse(cattleId, vaccineId, dosageInMl, date, vaccinationId);
        A.CallTo(() => _mapperMock.Map<VaccinationResponse>(vaccination)).Returns(expectedVaccinationResponse);

        VaccinationResponse vaccinationResponse = await _sut.EditVaccinationAsync(vaccinationRequest, vaccinationId, userId);

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