using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatetleManager.Application.Domain.Entities;
using CattleManager.Application.Application.Common.Enums;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.CattleBreeds;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.Owners;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Application.Services.General;
using CattleManager.Application.Domain.Entities;
using CattleManager.Tests.Providers;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CattleManager.Tests;

public class CattleServiceTests
{
    private readonly ICattleService _sut;
    private readonly ICattleRepository _cattleRepositoryMock;
    private readonly IGuidProvider _guidProvider;
    private readonly IGuidProvider _guidProviderMock;
    private static readonly Guid _girId = Guid.NewGuid();
    private static readonly Guid _holandesId = Guid.NewGuid();
    public CattleServiceTests()
    {
        _cattleRepositoryMock = A.Fake<ICattleRepository>();
        _guidProvider = new GuidProvider();
        _sut = new CattleService(_cattleRepositoryMock, _guidProvider);
        _guidProviderMock = A.Fake<IGuidProvider>();
    }

    [Fact]
    public async Task Get_All_Cattle_From_Owner_Returns_Empty_List_If_User_Has_No_Cattle()
    {
        List<Cattle> cattleFromDifferentOwner = new();
        for (int i = 0; i < 5; i++)
            cattleFromDifferentOwner.Add(GenerateCattle());
        Guid userId = Guid.NewGuid();
        Guid differentUserId = Guid.NewGuid();
        A.CallTo(() => _cattleRepositoryMock.GetAmountOfPages(differentUserId)).Returns(1);
        A.CallTo(() => _cattleRepositoryMock.GetAllCattleFromOwner(userId, 1)).Returns(cattleFromDifferentOwner);
        PaginatedCattleResponse expectedCattleResponse = new(new List<CattleResponse>(), 1, 1);

        PaginatedCattleResponse cattleResponse = await _sut.GetAllCattleFromOwner(differentUserId, 1);

        Assert.Equivalent(expectedCattleResponse, cattleResponse);
    }

    [Fact]
    public async Task Get_All_Cattle_From_Owners_That_Own_One_Or_More_Cattle_Returns_List_Of_Owned_Cattle()
    {
        List<Cattle> ownedCattle = new();
        for (int i = 0; i < 5; i++)
            ownedCattle.Add(GenerateCattle());
        Guid userId = Guid.NewGuid();
        A.CallTo(() => _cattleRepositoryMock.GetAmountOfPages(userId)).Returns(1);
        A.CallTo(() => _cattleRepositoryMock.GetAllCattleFromOwner(userId, 1)).Returns(ownedCattle);
        List<CattleResponse> ownedCattleByUser = new();
        foreach (Cattle animal in ownedCattle)
        {
            ownedCattleByUser.Add(GenerateCattleResponseDto(animal));
        }
        PaginatedCattleResponse expectedOwnedCattleResponse = new(ownedCattleByUser, 1, 1);

        var cattleResponse = await _sut.GetAllCattleFromOwner(userId, 1);

        Assert.Equivalent(expectedOwnedCattleResponse, cattleResponse);
    }

    [Fact]
    public async Task Get_Cattle_By_Non_Existent_Id_Throws_NotFoundException()
    {
        Guid cattleIdToBeSearched = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleIdToBeSearched, userId, true)).Returns(nullCattle);

        async Task result() => await _sut.GetCattleById(cattleIdToBeSearched, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o ID especificado não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Get_Cattle_By_Existent_Id_Returns_Cattle_Data()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle cattleFromOwner = GenerateCattle(cattleId, userId);
        CattleResponse cattleFromOwnerResponse = GenerateCattleResponseDto(cattleFromOwner);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(cattleFromOwner);

        var cattle = await _sut.GetCattleById(cattleId, userId);

        Assert.NotNull(cattle);
        Assert.Equivalent(cattleFromOwnerResponse, cattle, strict: true);
    }

    [Fact]
    public async Task Get_Cattle_By_Non_Existent_Name_Returns_Empty_List()
    {
        const string cattleName = "nonExistentName";
        Guid userId = Guid.NewGuid();
        List<Cattle> emptyList = new();
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleName, userId)).Returns(emptyList);

        var cattle = await _sut.GetCattleByNameAsync(cattleName, userId);

        Assert.Empty(cattle);
    }

    [Fact]
    public async Task Get_Cattle_By_Existing_Name_Returns_Cattle_Data()
    {
        const string cattleName = "existentName";
        Guid userId = Guid.NewGuid();
        List<Cattle> listOfCattleWithName = new();
        List<CattleResponse> listOfCattleWithNameResponse = new();
        for (int i = 0; i < 3; i++)
        {
            listOfCattleWithName.Add(GenerateCattle());
            listOfCattleWithNameResponse.Add(GenerateCattleResponseDto(listOfCattleWithName[i]));
        }
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleName, userId)).Returns(listOfCattleWithName);

        var cattleResponse = await _sut.GetCattleByNameAsync(cattleName, userId);

        listOfCattleWithNameResponse.Should().BeEquivalentTo(cattleResponse);
    }

    [Fact]
    public async Task Get_Male_Cattle_By_Name_With_Empty_Name_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        string emptyMaleCattleName = string.Empty;

        async Task result() => await _sut.GetMaleCattleByName(emptyMaleCattleName, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Nome do animal deve ser especificado.", exception.Message);
    }

    [Fact]
    public async Task Get_Male_Cattle_By_Name_Returns_User_Owned_Male_Cattle()
    {
        Guid userId = Guid.NewGuid();
        Guid allMaleCattleId = Guid.NewGuid();
        const string cattleName = "cattleName";
        IEnumerable<DropdownData> expectedMaleCattleByName = GenerateDropdownDataResponse(allMaleCattleId);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleName, userId)).Returns(GenerateDropdownDataResponse(allMaleCattleId));

        IEnumerable<DropdownData> maleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedMaleCattleByName, maleCattleByName);
    }

    [Fact]
    public async Task Get_Male_Cattle_By_Name_With_Name_With_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid maleCattleId = Guid.NewGuid();
        const string cattleName = "cáttlêWìthÃccént";
        const string cattleNameWithoutAccent = "cattleWithAccent";
        IEnumerable<DropdownData> expectedMaleCattleByName = GenerateDropdownDataResponse(maleCattleId, cattleName);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedMaleCattleByName);

        var maleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedMaleCattleByName, maleCattleByName);
    }

    [Fact]
    public async Task Get_Male_Cattle_By_Name_With_Name_Without_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid maleCattleId = Guid.NewGuid();
        const string cattleNameWithoutAccent = "cattleWithAccent";
        const string cattleNameWithAccent = "cáttlêWìthÃccént";
        IEnumerable<DropdownData> expectedMaleCattleByName = GenerateDropdownDataResponse(maleCattleId, cattleNameWithAccent);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedMaleCattleByName);

        var maleCattleByName = await _sut.GetMaleCattleByName(cattleNameWithoutAccent, userId);

        Assert.Equivalent(expectedMaleCattleByName, maleCattleByName);
    }

    [Fact]
    public async Task Get_Female_Cattle_By_Name_With_Empty_Name_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        string emptyFemaleCattleName = string.Empty;

        async Task result() => await _sut.GetFemaleCattleByName(emptyFemaleCattleName, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Nome do animal deve ser especificado.", exception.Message);
    }

    [Fact]
    public async Task Get_Female_Cattle_By_Name_Returns_User_Owned_Female_Cattle()
    {
        Guid userId = Guid.NewGuid();
        Guid allMaleCattleId = Guid.NewGuid();
        const string cattleName = "cattleName";
        IEnumerable<DropdownData> expectedFemaleCattleByName = GenerateDropdownDataResponse(allMaleCattleId);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleName, userId)).Returns(GenerateDropdownDataResponse(allMaleCattleId));

        IEnumerable<DropdownData> femaleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedFemaleCattleByName, femaleCattleByName);
    }

    [Fact]
    public async Task Get_Female_Cattle_By_Name_With_Name_With_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid femaleCattleId = Guid.NewGuid();
        const string cattleName = "cáttlêWìthÃccént";
        const string cattleNameWithoutAccent = "cattleWithAccent";
        IEnumerable<DropdownData> expectedFemaleCattleByName = GenerateDropdownDataResponse(femaleCattleId, cattleName);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedFemaleCattleByName);

        var femaleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedFemaleCattleByName, femaleCattleByName);
    }

    [Fact]
    public async Task Get_Female_Cattle_By_Name_With_Name_Without_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid femaleCattleId = Guid.NewGuid();
        const string cattleNameWithoutAccent = "cattleWithAccent";
        const string cattleNameWithAccent = "cáttlêWìthÃccént";
        IEnumerable<DropdownData> expectedFemaleCattleByName = GenerateDropdownDataResponse(femaleCattleId, cattleNameWithAccent);
        A.CallTo(() => _cattleRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedFemaleCattleByName);

        var femaleCattleByName = await _sut.GetMaleCattleByName(cattleNameWithoutAccent, userId);

        Assert.Equivalent(expectedFemaleCattleByName, femaleCattleByName);
    }

    [Fact]
    public async Task Get_All_Children_From_Non_Existent_Cattle_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(nullCattle);

        async Task result() => await _sut.GetAllChildrenFromCattle(cattleId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_All_Children_From_Cattle_Returns_All_Children()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle cattle = GenerateCattle();
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(cattle);
        Guid cattleChildId = Guid.NewGuid();
        Cattle cattleChild = GenerateCattle(cattleChildId);
        List<Cattle> cattleChildren = new() { cattleChild };
        List<CattleResponse> expectedCattleResponse = new() { GenerateCattleResponseDto(cattleChild) };
        A.CallTo(() => _cattleRepositoryMock.GetAllChildrenFromCattleAsync(cattleId, userId, (Gender)cattle.SexId)).Returns(cattleChildren);

        IEnumerable<CattleResponse> cattleResponse = await _sut.GetAllChildrenFromCattle(cattleId, userId);

        Assert.Equivalent(expectedCattleResponse, cattleResponse);
    }

    [Fact]
    public async Task Register_Cattle_With_Already_Existing_Name_And_Is_Owned_By_User_Throws_ConflictException()
    {
        Guid userId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId, fatherId: Guid.NewGuid(), motherId: Guid.NewGuid());
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>() { new Cattle() });

        async Task result() => await _sut.CreateCattle(cattleRequest, userId);

        var exception = await Assert.ThrowsAsync<ConflictException>(result);
        Assert.Equal("Gado com esse nome já existe.", exception.Message);
    }

    [Fact]
    public async Task Register_Cattle_With_Non_Existent_Father_Id_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId, fatherId: Guid.NewGuid(), motherId: Guid.NewGuid());
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>());
        Cattle? nullCattleFather = null;
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleRequest.FatherId ?? Guid.NewGuid())).Returns(nullCattleFather);

        async Task result() => await _sut.CreateCattle(cattleRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Pai especificado não foi encontrado.", exception.Message);
    }

    [Fact]
    public async Task Register_Cattle_With_Female_Father_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId, fatherId: fatherId, motherId: null);
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>());
        Cattle cattleFather = GenerateCattle(cattleId: fatherId, userId: userId, isMale: false);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleRequest.FatherId ?? Guid.NewGuid())).Returns(cattleFather);

        async Task result() => await _sut.CreateCattle(cattleRequest, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Pai do animal não pode ser do sexo feminino.", exception.Message);
    }

    [Fact]
    public async Task Register_Cattle_With_Non_Existent_Mother_Throws_NotFoundException()
    {
        Guid userId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId, fatherId: null, motherId: Guid.NewGuid());
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>());
        Cattle? nullCattleMother = null;
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleRequest.MotherId ?? Guid.NewGuid())).Returns(nullCattleMother);

        async Task result() => await _sut.CreateCattle(cattleRequest, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Mãe especificada não foi encontrada.", exception.Message);
    }

    [Fact]
    public async Task Register_Cattle_With_Male_Mother_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId: userId, fatherId: null, motherId: motherId);
        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>());
        Cattle cattleMotherResponse = GenerateCattle(cattleId: motherId, userId: userId, isMale: true);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(cattleRequest.MotherId ?? Guid.NewGuid())).Returns(cattleMotherResponse);

        async Task result() => await _sut.CreateCattle(cattleRequest, userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Mãe do animal não pode ser do sexo masculino.", exception.Message);
    }

    [Fact]
    public async Task Register_Cattle_With_Valid_Data_Is_Successful_And_Returns_Correct_Object()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        CattleRequest cattleRequest = GenerateCattleRequest(userId, fatherId, motherId);

        A.CallTo(() => _cattleRepositoryMock.GetCattleByName(cattleRequest.Name, userId)).Returns(new List<Cattle>());
        Cattle father = GenerateCattle(fatherId, userId, true);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(fatherId)).Returns(father);
        Cattle mother = GenerateCattle(fatherId, userId, false);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(motherId)).Returns(mother);
        Cattle cattleFromCattleRequest = GenerateCattleFromCattleRequest(cattleRequest);
        A.CallTo(() => _guidProviderMock.NewGuid()).Returns(cattleFromCattleRequest.Id);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleFromCattleRequest.Id, userId, true)).Returns(cattleFromCattleRequest);
        CattleResponse cattleResponse = GenerateCattleResponseDto(cattleFromCattleRequest);

        // Act
        var cattle = await _sut.CreateCattle(cattleRequest, userId);

        // Assert
        Assert.Equivalent(cattleResponse, cattle);
    }

    [Fact]
    public async Task Edit_Cattle_With_Unregistered_Id_Throws_NotFoundException()
    {
        Guid cattleId = _guidProvider.NewGuid();
        Guid userId = Guid.NewGuid();
        EditCattleRequest editCattleRequest = GenerateEditCattleRequest(cattleId, userId, Guid.NewGuid(), Guid.NewGuid());
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(nullCattle);

        async Task result() => await _sut.EditCattle(editCattleRequest, userId, editCattleRequest.Id!.Value);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Gado com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Edit_Cattle_With_Itself_As_Parent_Throws_BadRequestException()
    {
        Guid userId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        EditCattleRequest cattleRequest = GenerateEditCattleRequest(cattleId, userId, fatherId: cattleId, null);
        Cattle cattleFromCattleRequest = GenerateCattleFromCattleRequest(cattleRequest, cattleId);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleFromCattleRequest.Id, userId, true)).Returns(cattleFromCattleRequest);

        async Task result() => await _sut.EditCattle(cattleRequest, userId, cattleId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Animal não pode ser pai ou mãe dele próprio.", exception.Message);
    }

    [Fact]
    public async Task Edit_Cattle_With_Valid_Data_Returns_Edited_Cattle()
    {
        Guid userId = Guid.NewGuid();
        Guid fatherId = Guid.NewGuid();
        Guid motherId = Guid.NewGuid();
        Guid cattleId = Guid.NewGuid();
        EditCattleRequest cattleRequest = GenerateEditCattleRequest(cattleId, userId, fatherId, motherId);

        Cattle cattle = GenerateCattleFromCattleRequest(cattleRequest, cattleId);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, true)).Returns(cattle);
        Cattle father = GenerateCattle(fatherId, userId, true);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(fatherId)).Returns(father);
        Cattle mother = GenerateCattle(fatherId, userId, false);
        A.CallTo(() => _cattleRepositoryMock.GetByIdAsync(motherId)).Returns(mother);
        Cattle cattleFromCattleRequest = GenerateCattleFromCattleRequest(cattleRequest, cattleId);
        A.CallTo(() => _guidProviderMock.NewGuid()).Returns(cattleFromCattleRequest.Id);
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleFromCattleRequest.Id, userId, true)).Returns(cattleFromCattleRequest);
        CattleResponse cattleResponse = GenerateCattleResponseDto(cattleFromCattleRequest);

        var cattleResult = await _sut.EditCattle(cattleRequest, userId, cattleId);

        Assert.Equivalent(cattleResponse, cattleResult);
    }

    [Fact]
    public async Task Delete_Cattle_With_Non_Existent_Id_Throws_NotFoundException()
    {
        Guid cattleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Cattle? nullCattle = null;
        A.CallTo(() => _cattleRepositoryMock.GetCattleById(cattleId, userId, false)).Returns(nullCattle);

        async Task result() => await _sut.DeleteCattle(cattleId, userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Animal com o id especificado não foi encontrado.", exception.Message);
    }

    private static Cattle GenerateCattle(Guid? cattleId = null, Guid? userId = null, bool isMale = false)
    {
        Guid randomCattleId = Guid.NewGuid();
        return new Cattle()
        {
            Id = cattleId ?? Guid.NewGuid(),
            Name = "Cattlename",
            SexId = isMale ? (byte)1 : (byte)0,
            Conceptions = new List<Conception>(),
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2020, 9, 1)),
            YearOfBirth = 2020,
            FatherId = Guid.NewGuid(),
            MotherId = Guid.NewGuid(),
            Breeds = new List<Breed>()
            {
                new Breed() { Id = _girId, Name = "Gir" },
                new Breed() { Id = _holandesId, Name = "Holandês" }
            },
            CattleBreeds = new List<CattleBreed>()
            {
                new CattleBreed()
                {
                    Breed = new Breed() { Id = _girId, Name = "Gir" },
                    QuantityInPercentage = .625m,
                    BreedId = _girId,
                    CattleId = cattleId ?? randomCattleId
                },
                new CattleBreed()
                {
                    Breed = new Breed() { Id = _girId, Name = "Gir" },
                    QuantityInPercentage = .375m,
                    BreedId = _holandesId,
                    CattleId = cattleId ?? randomCattleId
                }
            },
            CattleOwners = new List<CattleOwner>()
            {
                new CattleOwner()
                {
                    CattleId = cattleId ?? randomCattleId,
                    User = new User()
                    {
                        FirstName = "FirstName", LastName = "LastName", Email = "email@email.com"
                    }
                }
            },
            Users = new List<User>()
            {
                new User()
                {
                    Id = userId ?? Guid.NewGuid(),
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "email@email.com"
                }
            },
        };
    }

    private static CattleResponse GenerateCattleResponseDto(Cattle cattle)
    {
        return new CattleResponse(
            Id: cattle.Id,
            Name: cattle.Name,
            FatherId: cattle.FatherId,
            FatherName: cattle.Father?.Name,
            MotherId: cattle.MotherId,
            MotherName: cattle.Mother?.Name,
            Sex: ((Gender)cattle.SexId).ToString(),
            cattle.CattleBreeds.
            Select(x => new CattleBreedResponse(
                x.Breed.Name, DecimalToFractionService.RealToFraction((double)x.QuantityInPercentage))),
            PurchaseDate: cattle.PurchaseDate,
            DateOfBirth: cattle.DateOfBirth,
            YearOfBirth: cattle.YearOfBirth,
            Image: cattle.Image,
            DateOfDeath: cattle.DateOfDeath,
            CauseOfDeath: cattle.CauseOfDeath,
            DateOfSale: cattle.DateOfSale,
            Owners: cattle.CattleOwners.Select(x => new CattleOwnerResponse(x.User.FirstName, x.User.LastName))
        );
    }

    private static IEnumerable<DropdownData> GenerateDropdownDataResponse(Guid cattleId, string cattleName = "Cattle name")
    {
        return new List<DropdownData>()
        {
            new DropdownData() { Text = cattleName, Value = cattleId }
        };
    }

    private static CattleRequest GenerateCattleRequest(Guid userId, Guid? fatherId, Guid? motherId)
    {
        return new CattleRequest()
        {
            Name = "Jupiter",
            FatherId = fatherId,
            MotherId = motherId,
            SexId = 0,
            Breeds = new List<CattleBreedRequest>()
            {
                new CattleBreedRequest(BreedId: _girId, QuantityInPercentage: .625m),
                new CattleBreedRequest(BreedId: _holandesId, QuantityInPercentage: .375m)
            },
            PurchaseDate = null,
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2020, 09, 01)),
            YearOfBirth = 2020,
            Image = null,
            DateOfDeath = null,
            CauseOfDeath = null,
            DateOfSale = null,
            PriceInCentsInReais = null,
            OwnersIds = new Guid[] { userId }
        };
    }

    private Cattle GenerateCattleFromCattleRequest(ICattleRequest cattleRequest, Guid? id = null)
    {
        Guid cattleId = _guidProvider.NewGuid();
        return new()
        {
            Id = id ?? cattleId,
            Name = cattleRequest.Name,
            FatherId = cattleRequest.FatherId,
            Father = new Cattle() { Id = Guid.NewGuid(), Name = "Father", SexId = 1 },
            MotherId = cattleRequest.MotherId,
            Mother = new Cattle() { Id = Guid.NewGuid(), Name = "Mother" },
            SexId = cattleRequest.SexId,
            PurchaseDate = cattleRequest.PurchaseDate,
            DateOfBirth = cattleRequest.DateOfBirth,
            YearOfBirth = cattleRequest.YearOfBirth,
            Image = cattleRequest.Image,
            DateOfDeath = cattleRequest.DateOfDeath,
            CauseOfDeath = cattleRequest.CauseOfDeath,
            DateOfSale = cattleRequest.DateOfSale,
            PriceInCentsInReais = cattleRequest.PriceInCentsInReais,
            CattleBreeds = new List<CattleBreed>()
            {
                new CattleBreed()
                {
                    Breed = new Breed() { Id = _girId, Name = "Gir" },
                    QuantityInPercentage = .625m,
                    BreedId = _girId,
                    CattleId = cattleId
                },
                new CattleBreed()
                {
                    Breed = new Breed() { Id = _girId, Name = "Gir" },
                    QuantityInPercentage = .375m,
                    BreedId = _holandesId,
                    CattleId = cattleId
                }
            },
            CattleOwners = new List<CattleOwner>()
            {
                new CattleOwner()
                {
                    CattleId = cattleId,
                    User = new User()
                    {
                        FirstName = "FirstName", LastName = "LastName", Email = "email@email.com"
                    }
                }
            },
        };
    }

    private static EditCattleRequest GenerateEditCattleRequest(Guid cattleId, Guid userId, Guid? fatherId = null, Guid? motherId = null)
    {
        return new EditCattleRequest()
        {
            Id = cattleId,
            Name = "Jupiter",
            FatherId = fatherId,
            MotherId = motherId,
            SexId = 0,
            Breeds = new List<CattleBreedRequest>()
            {
                new CattleBreedRequest(BreedId: _girId, QuantityInPercentage: .625m),
                new CattleBreedRequest(BreedId: _holandesId, QuantityInPercentage: .375m)
            },
            PurchaseDate = null,
            DateOfBirth = DateOnly.FromDateTime(new DateTime(2020, 09, 01)),
            YearOfBirth = 2020,
            Image = null,
            DateOfDeath = null,
            CauseOfDeath = null,
            DateOfSale = null,
            PriceInCentsInReais = null,
            OwnersIds = new Guid[] { userId }
        };
    }
}
