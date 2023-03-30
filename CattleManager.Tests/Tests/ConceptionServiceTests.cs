using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class ConceptionServiceTests
{
    private readonly IConceptionService _sut;
    private readonly IConceptionRepository _conceptionRepositoryMock;
    private readonly ICattleRepository _cattleRepositoryMock;
    private readonly IMapper _mapperMock;
    public ConceptionServiceTests()
    {
        _conceptionRepositoryMock = A.Fake<IConceptionRepository>();
        _cattleRepositoryMock = A.Fake<ICattleRepository>();
        _mapperMock = A.Fake<IMapper>();
        _sut = new ConceptionService(_conceptionRepositoryMock, _cattleRepositoryMock, _mapperMock);
    }

    [Fact]
    public async Task Get_Conception_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Guid conceptionId = Guid.NewGuid();
        Conception? nullConception = null;
        A.CallTo(() => _conceptionRepositoryMock.GetByIdAsync(conceptionId)).Returns(nullConception);

        async Task result() => await _sut.GetConceptionByIdAsync(conceptionId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Concepção com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Conception_By_Existend_Id_Returns_Conception()
    {
        Guid conceptionId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Conception conception = GenerateConception(conceptionId, fatherId, motherId);
        A.CallTo(() => _conceptionRepositoryMock.GetByIdAsync(conceptionId)).Returns(conception);
        ConceptionResponse expectedConceptionResponse = GenerateConceptionResponseFromConception(conception);
        A.CallTo(() => _mapperMock.Map<ConceptionResponse>(conception)).Returns(expectedConceptionResponse);

        ConceptionResponse conceptionResponse = await _sut.GetConceptionByIdAsync(conceptionId);

        Assert.Equivalent(expectedConceptionResponse, conceptionResponse);
    }

    [Fact]
    public async Task Get_All_Conceptions_From_Non_ExistentCattle_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(nullCattle);

        async Task result() => await _sut.GetAllConceptionsFromCattleAsync(cattleId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_All_Conceptions_From_Cattle_Returns_Empty_List_If_Cattle_Has_No_Conceptions()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(new Cattle());
        List<Conception> emptyList = new();
        A.CallTo(() => _conceptionRepositoryMock.GetAllConceptionsFromCattle(cattleId, userId)).Returns(emptyList);
        A.CallTo(() => _mapperMock.Map<List<ConceptionResponse>>(emptyList)).Returns(new List<ConceptionResponse>());

        IEnumerable<ConceptionResponse> conceptions = await _sut.GetAllConceptionsFromCattleAsync(cattleId, userId);

        Assert.Empty(conceptions);
    }

    [Fact]
    public async Task Get_All_Conceptions_From_Cattle_Returns_List_Of_Conceptions()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        List<Conception> conceptionsList = new();
        List<ConceptionResponse> conceptionResponsesList = new();
        for (int i = 0; i < 5; i++)
        {
            Conception conception = GenerateConception(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            conceptionsList.Add(conception);
            conceptionResponsesList.Add(GenerateConceptionResponseFromConception(conception));
        }
        A.CallTo(() => _conceptionRepositoryMock.GetAllConceptionsFromCattle(cattleId, userId)).Returns(conceptionsList);
        A.CallTo(() => _mapperMock.Map<List<ConceptionResponse>>(conceptionsList)).Returns(conceptionResponsesList);

        var conceptions = await _sut.GetAllConceptionsFromCattleAsync(cattleId, userId);

        Assert.Equivalent(conceptionResponsesList, conceptions);
    }

    [Fact]
    public async Task Create_Conception_With_Non_Existent_Mother_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle? nullCattleMother = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(nullCattleMother);
        CreateConceptionRequest conceptionRequest = GenerateCreateConceptionRequest(fatherId, motherId);

        async Task result() => await _sut.CreateConceptionAsync(conceptionRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Conception_With_Male_Mother_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle maleCattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Male,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(maleCattleMother);
        CreateConceptionRequest conceptionRequest = GenerateCreateConceptionRequest(fatherId, motherId);

        async Task result() => await _sut.CreateConceptionAsync(conceptionRequest, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Mãe não pode ser do sexo masculino.", exception.Message);
    }

    [Fact]
    public async Task Create_Conception_With_Non_Existent_Father_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);
        Cattle? nullFather = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(nullFather);
        CreateConceptionRequest conceptionRequest = GenerateCreateConceptionRequest(fatherId, motherId);

        async Task result() => await _sut.CreateConceptionAsync(conceptionRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Conception_With_Female_Father_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);
        Cattle femaleCattleFather = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(femaleCattleFather);
        CreateConceptionRequest conceptionRequest = GenerateCreateConceptionRequest(fatherId, motherId);

        async Task result() => await _sut.CreateConceptionAsync(conceptionRequest, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Pai não pode ser do sexo feminino.", exception.Message);
    }

    [Fact]
    public async Task Create_Conception_Returns_Created_Conception()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();

        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);

        Cattle cattleFather = new()
        {
            Id = motherId,
            SexId = (int)Gender.Male,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(cattleFather);

        CreateConceptionRequest conceptionRequest = GenerateCreateConceptionRequest(fatherId, motherId);
        Guid conceptionId = Guid.NewGuid();
        Conception conception = GenerateConception(conceptionId, fatherId, motherId);
        A.CallTo(() => _mapperMock.Map<Conception>(conceptionRequest)).Returns(conception);
        ConceptionResponse expectedConceptionResponse = GenerateConceptionResponseFromConception(conception);
        A.CallTo(() => _mapperMock.Map<ConceptionResponse>(conception)).Returns(expectedConceptionResponse);

        // Act
        ConceptionResponse conceptionResponse = await _sut.CreateConceptionAsync(conceptionRequest, userId);

        // Assert
        Assert.Equivalent(expectedConceptionResponse, conceptionResponse);
    }

    [Fact]
    public async Task Edit_Conception_With_Different_Route_Id_Than_Specified_Throws_BadRequestException()
    {
        Guid conceptionId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);
        Guid userId = Guid.NewGuid();

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, routeId: Guid.NewGuid());

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Rota não coincide com o id especificado.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_With_Non_Existent_Conception_Throws_NotFoundException()
    {
        Guid conceptionId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);
        Guid userId = Guid.NewGuid();
        Conception? nullConception = null;
        A.CallTo(() => _conceptionRepositoryMock.GetConceptionByIdAsync(conceptionId, false)).Returns(nullConception);

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Concepção com o id especificado não foi encontrada.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_With_Non_Existent_Mother_Throws_NotFoundException()
    {
        Guid conceptionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        Cattle? nullMother = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(nullMother);
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_With_Male_Mother_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid conceptionId = Guid.NewGuid();
        Cattle maleCattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Male,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(maleCattleMother);
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Mãe não pode ser do sexo masculino.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_With_Non_Existent_Father_Throws_NotFoundException()
    {
        Guid conceptionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);
        Cattle? nullFather = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(nullFather);
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_With_Female_Father_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid conceptionId = Guid.NewGuid();
        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);
        Cattle femaleCattleFather = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(femaleCattleFather);
        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);

        async Task result() => await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Pai não pode ser do sexo feminino.", exception.Message);
    }

    [Fact]
    public async Task Edit_Conception_Returns_Edited_Conception()
    {
        // Arrange
        Guid conceptionId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();

        Cattle cattleMother = new()
        {
            Id = motherId,
            SexId = (int)Gender.Female,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(motherId, userId, true)).Returns(cattleMother);

        Cattle cattleFather = new()
        {
            Id = motherId,
            SexId = (int)Gender.Male,
            Users = new List<User>() { new User() { Id = userId } }
        };
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(fatherId, userId, true)).Returns(cattleFather);

        EditConceptionRequest editConceptionRequest = GenerateEditConceptionRequest(conceptionId, fatherId, motherId);
        Conception conception = GenerateConception(conceptionId, fatherId, motherId);
        A.CallTo(() => _mapperMock.Map<Conception>(editConceptionRequest)).Returns(conception);
        ConceptionResponse expectedConceptionResponse = GenerateConceptionResponseFromConception(conception);
        A.CallTo(() => _mapperMock.Map<ConceptionResponse>(conception)).Returns(expectedConceptionResponse);

        // Act
        ConceptionResponse conceptionResponse = await _sut.EditConceptionAsync(editConceptionRequest, userId, conceptionId);

        // Assert
        Assert.Equivalent(expectedConceptionResponse, conceptionResponse);
    }

    private static Conception GenerateConception(Guid conceptionId, Guid fatherId, Guid motherId)
    {
        return new Conception()
        {
            Id = conceptionId,
            Date = DateOnly.FromDateTime(DateTime.Now),
            FatherId = fatherId,
            MotherId = motherId
        };
    }

    private static ConceptionResponse GenerateConceptionResponseFromConception(Conception conception)
    {
        return new ConceptionResponse(
            conception.Id,
            conception.Date,
            conception.FatherId,
            conception.MotherId);
    }

    private static CreateConceptionRequest GenerateCreateConceptionRequest(Guid fatherId, Guid motherId)
    {
        return new CreateConceptionRequest(DateOnly.FromDateTime(DateTime.Now), fatherId, motherId);
    }

    private static EditConceptionRequest GenerateEditConceptionRequest(Guid Id, Guid fatherId, Guid motherId)
    {
        return new EditConceptionRequest(Id, DateOnly.FromDateTime(DateTime.Now), fatherId, motherId);
    }
}