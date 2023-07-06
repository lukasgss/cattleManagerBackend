using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Common.Interfaces.Entities.Farms;
using CattleManager.Application.Domain.Entities;
using CattleManager.Application.Services.Entities;
using CattleManager.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class FarmServiceTests
{
    private readonly IFarmService _sut;
    private readonly IFarmRepository _farmRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IGuidProvider _guidProviderMock;
    private readonly IMapper _mapperMock;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _farmId = Guid.NewGuid();
    private readonly string _farmName = "Farm name";

    public FarmServiceTests()
    {
        _farmRepositoryMock = A.Fake<IFarmRepository>();
        _userRepositoryMock = A.Fake<IUserRepository>();
        _guidProviderMock = A.Fake<IGuidProvider>();
        _mapperMock = A.Fake<IMapper>();
        _sut = new FarmService(_farmRepositoryMock,
            _userRepositoryMock,
            _guidProviderMock,
            _mapperMock
        );
    }

    [Fact]
    public async Task Get_Farm_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Farm? nullFarm = null;
        A.CallTo(() => _farmRepositoryMock.GetFarmByIdAsync(_userId, _farmId, true)).Returns(nullFarm);

        async Task result() => await _sut.GetFarmByIdAsync(_userId, _farmId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Fazenda com o id especificado não foi encontrada.", exception.Message);
    }

    [Fact]
    public async Task Get_Farm_By_Id_Returns_Farm()
    {
        Farm searchedFarm = GenerateFarm();
        A.CallTo(() => _farmRepositoryMock.GetFarmByIdAsync(_userId, _farmId, true)).Returns(searchedFarm);
        FarmResponse expectedFarmResponse = GenerateFarmResponseFromFarm(searchedFarm);
        A.CallTo(() => _mapperMock.Map<FarmResponse>(searchedFarm)).Returns(expectedFarmResponse);

        FarmResponse farmResponse = await _sut.GetFarmByIdAsync(_userId, _farmId);

        Assert.Equivalent(expectedFarmResponse, farmResponse);
    }

    [Fact]
    public async Task Create_Farm_With_Non_Existent_User_Throws_NotFoundException()
    {
        User? nullUser = null;
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_userId)).Returns(nullUser);

        async Task result() => await _sut.CreateFarmAsync(_userId, GenerateCreateFarmRequest());

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Usuário para criar a fazenda não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Create_Farm_Returns_Created_Farm()
    {
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_userId)).Returns(new User());
        A.CallTo(() => _guidProviderMock.NewGuid()).Returns(_farmId);
        CreateFarmRequest createFarmRequest = GenerateCreateFarmRequest();
        Farm createdFarm = GenerateFarm();
        FarmResponse expectedFarmResponse = GenerateFarmResponseFromFarm(createdFarm);

        FarmResponse farmResponse = await _sut.CreateFarmAsync(_userId, createFarmRequest);

        Assert.Equivalent(expectedFarmResponse, farmResponse);
    }

    [Fact]
    public async Task Edit_Farm_With_Route_Id_Different_Than_Request_Body_Id_Throws_BadRequestException()
    {
        Guid routeId = Guid.NewGuid();
        EditFarmRequest editFarmRequest = GenerateEditFarmRequest();

        async Task result() => await _sut.EditFarmAsync(_userId, editFarmRequest, routeId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Id da rota não coincide com o id da fazenda especificada.", exception.Message);
    }

    [Fact]
    public async Task Edit_Non_Existent_Farm_Throws_NotFoundException()
    {
        EditFarmRequest editFarmRequest = GenerateEditFarmRequest();
        Farm? nullFarm = null;
        A.CallTo(() => _farmRepositoryMock.GetFarmByIdAsync(_userId, _farmId, false)).Returns(nullFarm);

        async Task result() => await _sut.EditFarmAsync(_userId, editFarmRequest, _farmId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Fazenda com o id especificado não foi encontrada.", exception.Message);
    }

    [Fact]
    public async Task Edit_Farm_Returns_Edited_Farm()
    {
        EditFarmRequest editFarmRequest = GenerateEditFarmRequest();
        Farm farmToEdit = GenerateFarm();
        A.CallTo(() => _farmRepositoryMock.GetFarmByIdAsync(_userId, _farmId, false)).Returns(farmToEdit);
        FarmResponse expectedFarmResponse = GenerateFarmResponseFromFarm(farmToEdit);

        FarmResponse farmResponse = await _sut.EditFarmAsync(_userId, editFarmRequest, _farmId);

        Assert.Equivalent(expectedFarmResponse, farmResponse);
    }

    [Fact]
    public async Task Delete_Non_Existent_Farm_Throws_NotFoundException()
    {
        Farm? nullFarm = null;
        A.CallTo(() => _farmRepositoryMock.GetFarmByIdAsync(_userId, _farmId, true)).Returns(nullFarm);

        async Task result() => await _sut.DeleteFarmAsync(_farmId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Fazenda com o id especificado não foi encontrada.", exception.Message);
    }

    private EditFarmRequest GenerateEditFarmRequest()
    {
        return new EditFarmRequest(_farmId, _farmName);
    }

    private CreateFarmRequest GenerateCreateFarmRequest()
    {
        return new CreateFarmRequest(_farmName);
    }

    private Farm GenerateFarm()
    {
        return new Farm()
        {
            Id = _farmId,
            Name = _farmName,
            Owners = new List<FarmOwner>()
            {
                new FarmOwner() { OwnerId = _userId, FarmId = _farmId }
            }
        };
    }

    private static FarmResponse GenerateFarmResponseFromFarm(Farm farm)
    {
        return new FarmResponse()
        {
            Id = farm.Id,
            Name = farm.Name
        };
    }
}