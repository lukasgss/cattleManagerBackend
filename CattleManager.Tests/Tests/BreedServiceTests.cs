using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using CattleManager.Application.Application.Services.Entities;
using FakeItEasy;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace CattleManager.Tests.Tests;

public class BreedServiceTests
{
    private readonly IBreedService _sut;
    private readonly IMemoryCache _memoryCacheMock;
    private readonly IBreedRepository _breedRepositoryMock;

    public BreedServiceTests()
    {
        _breedRepositoryMock = A.Fake<IBreedRepository>();
        _memoryCacheMock = A.Fake<IMemoryCache>();
        _sut = new BreedService(_breedRepositoryMock, _memoryCacheMock);
    }

    [Fact]
    public async Task Get_All_Breeds_For_Dropdown_Returns_All_Breeds()
    {
        Guid breedId = Guid.NewGuid();
        IEnumerable<DropdownData> expectedBreeds = GenerateBreeds(breedId);
        A.CallTo(() => _breedRepositoryMock.GetAllBreedsForDropdown()).Returns(expectedBreeds);

        var breeds = await _sut.GetAllBreedsForDropdown();

        Assert.Equivalent(expectedBreeds, breeds);
    }

    private static IEnumerable<DropdownData> GenerateBreeds(Guid breedId)
    {
        return new List<DropdownData>() { new DropdownData() { Text = "Gir", Value = breedId } };
    }
}