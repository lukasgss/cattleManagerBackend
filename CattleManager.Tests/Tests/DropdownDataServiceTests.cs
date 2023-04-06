using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Services.FrontendDropdownData;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class DropdownDataServiceTests
{
    private readonly IDropdownDataService _sut;
    private readonly IDropdownDataRepository _dropdownDataRepositoryMock;
    public DropdownDataServiceTests()
    {
        _dropdownDataRepositoryMock = A.Fake<IDropdownDataRepository>();
        _sut = new DropdownDataService(_dropdownDataRepositoryMock);
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
        IEnumerable<DropdownDataResponse> expectedMaleCattleByName = GenerateDropdownDataResponse(allMaleCattleId);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleName, userId)).Returns(GenerateDropdownDataResponse(allMaleCattleId));

        IEnumerable<DropdownDataResponse> maleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedMaleCattleByName, maleCattleByName);
    }

    [Fact]
    public async Task Get_Male_Cattle_By_Name_With_Name_With_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid maleCattleId = Guid.NewGuid();
        const string cattleName = "cáttlêWìthÃccént";
        const string cattleNameWithoutAccent = "cattleWithAccent";
        IEnumerable<DropdownDataResponse> expectedMaleCattleByName = GenerateDropdownDataResponse(maleCattleId, cattleName);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedMaleCattleByName);

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
        IEnumerable<DropdownDataResponse> expectedMaleCattleByName = GenerateDropdownDataResponse(maleCattleId, cattleNameWithAccent);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedMaleCattleByName);

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
        IEnumerable<DropdownDataResponse> expectedFemaleCattleByName = GenerateDropdownDataResponse(allMaleCattleId);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleName, userId)).Returns(GenerateDropdownDataResponse(allMaleCattleId));

        IEnumerable<DropdownDataResponse> femaleCattleByName = await _sut.GetMaleCattleByName(cattleName, userId);

        Assert.Equivalent(expectedFemaleCattleByName, femaleCattleByName);
    }

    [Fact]
    public async Task Get_Female_Cattle_By_Name_With_Name_With_Accents_Returns_Cattle_With_Name_With_Accents()
    {
        Guid userId = Guid.NewGuid();
        Guid femaleCattleId = Guid.NewGuid();
        const string cattleName = "cáttlêWìthÃccént";
        const string cattleNameWithoutAccent = "cattleWithAccent";
        IEnumerable<DropdownDataResponse> expectedFemaleCattleByName = GenerateDropdownDataResponse(femaleCattleId, cattleName);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedFemaleCattleByName);

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
        IEnumerable<DropdownDataResponse> expectedFemaleCattleByName = GenerateDropdownDataResponse(femaleCattleId, cattleNameWithAccent);
        A.CallTo(() => _dropdownDataRepositoryMock.GetMaleCattleByName(cattleNameWithoutAccent, userId)).Returns(expectedFemaleCattleByName);

        var femaleCattleByName = await _sut.GetMaleCattleByName(cattleNameWithoutAccent, userId);

        Assert.Equivalent(expectedFemaleCattleByName, femaleCattleByName);
    }

    private static IEnumerable<DropdownDataResponse> GenerateDropdownDataResponse(Guid cattleId, string cattleName = "Cattle name")
    {
        return new List<DropdownDataResponse>()
        {
            new DropdownDataResponse() { Text = cattleName, Value = cattleId }
        };
    }
}