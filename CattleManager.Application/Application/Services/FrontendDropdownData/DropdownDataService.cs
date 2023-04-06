using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;

namespace CattleManager.Application.Application.Services.FrontendDropdownData;

public class DropdownDataService : IDropdownDataService
{
    private readonly IDropdownDataRepository _dropdownDataRepository;
    public DropdownDataService(IDropdownDataRepository dropdownDataRepository)
    {
        _dropdownDataRepository = dropdownDataRepository;
    }

    public async Task<IEnumerable<DropdownDataResponse>> GetMaleCattleByName(string name, Guid userId)
    {
        if (name?.Length == 0)
            throw new BadRequestException("Nome do animal deve ser especificado.");

        return await _dropdownDataRepository.GetMaleCattleByName(name!, userId);
    }
}