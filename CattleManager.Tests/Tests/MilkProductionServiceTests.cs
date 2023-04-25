using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using CattleManager.Tests.Providers;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class MilkProductionServiceTests
{
    private readonly IMilkProductionService _sut;
    private readonly IMilkProductionRepository _milkProductionRepositoryMock;
    private readonly ICattleRepository _cattleRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly IGuidProvider _guidProvider;
    private readonly IServiceValidations _serviceValidationsMock;
    private static readonly Guid _userId = Guid.NewGuid();
    private static readonly Guid _cattleId = Guid.NewGuid();

    public MilkProductionServiceTests()
    {
        _milkProductionRepositoryMock = A.Fake<IMilkProductionRepository>();
        _cattleRepositoryMock = A.Fake<ICattleRepository>();
        _mapperMock = A.Fake<IMapper>();
        _serviceValidationsMock = A.Fake<IServiceValidations>();
        _guidProvider = new GuidProvider();
        _sut = new MilkProductionService(
            _milkProductionRepositoryMock,
            _cattleRepositoryMock,
            _mapperMock,
            _serviceValidationsMock);
    }

    [Fact]
    public async Task Get_Milk_Production_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        MilkProduction? nullMilkProduction = null;
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, true)).Returns(nullMilkProduction);

        async Task result() => await _sut.GetMilkProductionByIdAsync(milkProductionId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Não foi possível encontrar produção de leite com o id especificado.", exception.Message);
    }

    [Fact]
    public async Task Get_Milk_Production_By_Existent_Id_Returns_Milk_Production()
    {
        Guid cattleId = Guid.NewGuid();
        Guid milkProductionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        MilkProduction milkProduction = GenerateMilkProduction(cattleId, milkProductionId);
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, true)).Returns(milkProduction);
        MilkProductionResponse expectedMilkProductionResponse = GenerateMilkProductionResponseFromMilkProduction(milkProduction, milkProductionId);
        A.CallTo(() => _mapperMock.Map<MilkProductionResponse>(milkProduction)).Returns(expectedMilkProductionResponse);

        var milkProductionResponse = await _sut.GetMilkProductionByIdAsync(milkProductionId, userId);

        Assert.Equivalent(expectedMilkProductionResponse, milkProductionResponse);
    }

    [Fact]
    public async Task Get_All_Milk_Productions_From_Cattle_With_Non_Existent_Cattle_Id_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(nullCattle);

        async Task result() => await _sut.GetAllMilkProductionsFromCattleAsync(cattleId, userId, 1);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Get_All_Milk_Productions_From_Cattle_With_Existent_Cattle_Id_Returns_All_Milk_Productions()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _milkProductionRepositoryMock.GetAmountOfPages(cattleId, userId)).Returns(1);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(new Cattle());
        List<MilkProduction> milkProductions = GenerateListOfMilkProductions(cattleId);
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionsFromCattleAsync(cattleId, userId, 1)).Returns(milkProductions);
        List<MilkProductionResponse> expectedMilkProductionResponse = GenerateListOfMilkProductionResponseFromListOfMilkProductions(milkProductions);
        A.CallTo(() => _mapperMock.Map<List<MilkProductionResponse>>(milkProductions)).Returns(expectedMilkProductionResponse);
        PaginatedMilkProductionResponse expectedResponse = new(expectedMilkProductionResponse, 1, 1);

        var milkProductionResponse = await _sut.GetAllMilkProductionsFromCattleAsync(cattleId, userId, 1);

        Assert.Equivalent(expectedResponse, milkProductionResponse);
    }

    [Fact]
    public async Task Get_Average_Milk_Production_From_All_Cattle_Returns_Average_From_All_Cattle()
    {
        const int month = 1;
        const int year = 1;
        DateTime currentDate = new(2023, month, year);
        AverageOfEntity expectedAverageMilkProduction = new()
        {
            Average = 30,
            Quantity = 400
        };
        A.CallTo(() => _milkProductionRepositoryMock.GetAverageMilkProductionFromAllCattleAsync(_userId, month, year)).Returns(expectedAverageMilkProduction);

        AverageOfEntity averageMilkProduction = await _sut.GetAverageMilkProductionFromAllCattleAsync(_userId, month, year);

        Assert.Equivalent(expectedAverageMilkProduction, averageMilkProduction);
    }

    [Fact]
    public async Task Get_Average_Milk_Production_From_Cattle_Returns_Average_Milk_Production()
    {
        const int month = 1;
        const int year = 1;
        DateTime currentDate = new(2023, month, year);
        AverageMilkProduction expectedAverageMilkProduction = new()
        {
            Average = 30,
        };
        A.CallTo(() => _milkProductionRepositoryMock.GetAverageMilkProductionFromCattleAsync(_cattleId, _userId, month, year)).Returns(expectedAverageMilkProduction);

        AverageMilkProduction averageMilkProduction = await _sut.GetAverageMilkProductionFromCattleAsync(_cattleId, _userId, month, year);

        Assert.Equivalent(expectedAverageMilkProduction, averageMilkProduction);
    }

    [Fact]
    public async Task Create_Milk_Production_With_Non_Existent_Cattle_Id_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        MilkProductionRequest milkProductionRequest = GenerateMilkProductionRequest(cattleId);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(nullCattle);

        async Task result() => await _sut.CreateMilkProductionAsync(milkProductionRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Usuário não possui um animal com o id especificado.", exception.Message);
    }

    [Fact]
    public async Task Create_Milk_Production_With_Existent_Cattle_Id_Returns_Created_Milk_Production()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(new Cattle());
        MilkProductionRequest milkProductionRequest = GenerateMilkProductionRequest(cattleId);
        MilkProduction milkProduction = GenerateMilkProduction(cattleId);
        MilkProductionResponse expectedMilkProductionResponse = GenerateMilkProductionResponseFromMilkProduction(milkProduction);
        A.CallTo(() => _mapperMock.Map<MilkProduction>(milkProductionRequest)).Returns(milkProduction);
        A.CallTo(() => _mapperMock.Map<MilkProductionResponse>(milkProduction)).Returns(expectedMilkProductionResponse);

        var milkProductionResponse = await _sut.CreateMilkProductionAsync(milkProductionRequest, userId);

        Assert.Equivalent(expectedMilkProductionResponse, milkProductionResponse);
    }

    [Fact]
    public async Task Edit_Milk_Production_With_Different_Route_Id_From_Specified_In_Request_Throws_BadRequestException()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid routeId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        EditMilkProductionRequest editMilkProductionRequest = GenerateEditMilkProductionRequest(milkProductionId, cattleId);

        async Task result() => await _sut.EditMilkProductionByIdAsync(editMilkProductionRequest, userId, routeId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Id da rota e o especificado da produção de leite não coincidem.", exception.Message);
    }

    [Fact]
    public async Task Edit_Milk_Production_With_Non_Existent_Milk_Production_Id_Throws_NotFoundException()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        EditMilkProductionRequest editMilkProductionRequest = GenerateEditMilkProductionRequest(milkProductionId, cattleId);
        MilkProduction? nullMilkProduction = null;
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, true)).Returns(nullMilkProduction);

        async Task result() => await _sut.EditMilkProductionByIdAsync(editMilkProductionRequest, userId, editMilkProductionRequest.Id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Produção de leite com o id especificado não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Edit_Milk_Production_With_Non_Existent_Cattle_Id_Throws_NotFoundException()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        EditMilkProductionRequest editMilkProductionRequest = GenerateEditMilkProductionRequest(milkProductionId, cattleId);
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, true)).Returns(new MilkProduction());
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleId)).Returns(nullCattle);

        async Task result() => await _sut.EditMilkProductionByIdAsync(editMilkProductionRequest, userId, editMilkProductionRequest.Id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Edit_Milk_Production_With_Valid_Payload_Returns_Milk_Production_Response()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        EditMilkProductionRequest editMilkProductionRequest = GenerateEditMilkProductionRequest(milkProductionId, cattleId);
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, true)).Returns(new MilkProduction());
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleId)).Returns(new Cattle());
        MilkProduction milkProduction = GenerateMilkProduction(cattleId, milkProductionId);
        A.CallTo(() => _mapperMock.Map<MilkProduction>(editMilkProductionRequest)).Returns(milkProduction);
        MilkProductionResponse expectedMilkProductionResponse = GenerateMilkProductionResponseFromMilkProduction(milkProduction);
        A.CallTo(() => _mapperMock.Map<MilkProductionResponse>(milkProduction)).Returns(expectedMilkProductionResponse);

        var milkProductionResponse = await _sut.EditMilkProductionByIdAsync(editMilkProductionRequest, userId, editMilkProductionRequest.Id);

        Assert.Equivalent(expectedMilkProductionResponse, milkProductionResponse);
    }

    [Fact]
    public async Task Delete_Milk_Production_With_Non_Existent_Id_Throws_NotFoundException()
    {
        Guid milkProductionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        MilkProduction? nullMilkProduction = null;
        A.CallTo(() => _milkProductionRepositoryMock.GetMilkProductionByIdAsync(milkProductionId, userId, false)).Returns(nullMilkProduction);

        async Task result() => await _sut.DeleteMilkProductionByIdAsync(milkProductionId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Produção de leite com o id especificado não foi encontrado.", exception.Message);
    }

    private MilkProduction GenerateMilkProduction(Guid cattleId, Guid? milkProductionId = null)
    {
        return new MilkProduction()
        {
            Id = milkProductionId ?? _guidProvider.NewGuid(),
            CattleId = cattleId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            MilkInLiters = 15
        };
    }

    private List<MilkProduction> GenerateListOfMilkProductions(Guid cattleId)
    {
        List<MilkProduction> milkProductions = new();
        for (int i = 0; i < 10; i++)
        {
            milkProductions.Add(GenerateMilkProduction(cattleId));
        }
        return milkProductions;
    }

    private MilkProductionResponse GenerateMilkProductionResponseFromMilkProduction(MilkProduction milkProduction, Guid? milkProductionId = null)
    {
        return new MilkProductionResponse(
            Id: milkProductionId ?? _guidProvider.NewGuid(),
            MilkInLiters: milkProduction.MilkInLiters,
            PeriodOfDay: "afternoon",
            Date: milkProduction.Date,
            CattleId: milkProduction.CattleId);
    }

    private List<MilkProductionResponse> GenerateListOfMilkProductionResponseFromListOfMilkProductions(List<MilkProduction> milkProductions)
    {
        List<MilkProductionResponse> milkProductionResponses = new();
        foreach (MilkProduction milkProduction in milkProductions)
        {
            milkProductionResponses.Add(GenerateMilkProductionResponseFromMilkProduction(milkProduction, milkProduction.Id));
        }
        return milkProductionResponses;
    }

    private static MilkProductionRequest GenerateMilkProductionRequest(Guid cattleId)
    {
        return new MilkProductionRequest(
            MilkInLiters: 15,
            PeriodOfDay: "afternoon",
            Date: DateOnly.FromDateTime(DateTime.Now),
            CattleId: cattleId
        );
    }

    private static EditMilkProductionRequest GenerateEditMilkProductionRequest(Guid milkProductionId, Guid? cattleId = null)
    {
        return new EditMilkProductionRequest(
            Id: milkProductionId,
            MilkInLiters: 15,
            PeriodOfDay: "afternoon",
            Date: DateOnly.FromDateTime(DateTime.Now),
            CattleId: cattleId ?? Guid.NewGuid()
        );
    }
}