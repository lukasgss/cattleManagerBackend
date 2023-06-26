using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

namespace CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;

public interface IBreedService
{
    Task<IEnumerable<DropdownData>> GetAllBreedsForDropdown();
}