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

    private static IEnumerable<DropdownDataResponse> GenerateDropdownDataResponse(Guid cattleId)
    {
        return new List<DropdownDataResponse>()
        {
            new DropdownDataResponse() { Text = "Cattle name", Value = cattleId }
        };
    }
}