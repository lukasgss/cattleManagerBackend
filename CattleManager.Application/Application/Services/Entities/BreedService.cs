using CattleManager.Application.Application.Cache;
using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using Microsoft.Extensions.Caching.Memory;

namespace CattleManager.Application.Application.Services.Entities;

public class BreedService : IBreedService
{
    private readonly IBreedRepository _breedRepository;
    private readonly IMemoryCache _memoryCache;

    public BreedService(IBreedRepository breedRepository, IMemoryCache memoryCache)
    {
        _breedRepository = breedRepository;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<DropdownData>> GetAllBreedsForDropdown()
    {
        IEnumerable<DropdownData>? allBreeds = _memoryCache.Get<IEnumerable<DropdownData>>(CacheKeys.Breeds);

        if (allBreeds is null)
        {
            allBreeds = await _breedRepository.GetAllBreedsForDropdown();
            _memoryCache.Set(CacheKeys.Breeds, allBreeds);
        }

        return allBreeds;
    }
}