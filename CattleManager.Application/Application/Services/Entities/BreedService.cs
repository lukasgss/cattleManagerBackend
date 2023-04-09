using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

namespace CattleManager.Application.Application.Services.Entities;

public class BreedService : IBreedService
{
    private readonly IBreedRepository _breedRepository;

    public BreedService(IBreedRepository breedRepository)
    {
        _breedRepository = breedRepository;
    }

    public async Task<IEnumerable<DropdownData>> GetAllBreedsForDropdown()
    {
        return await _breedRepository.GetAllBreedsForDropdown();
    }
}