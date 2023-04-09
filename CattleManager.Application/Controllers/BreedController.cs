using CattleManager.Application.Application.Common.Interfaces.Entities.Breeds;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/breeds")]
public class BreedController : ControllerBase
{
    private readonly IBreedService _breedService;

    public BreedController(IBreedService breedService)
    {
        _breedService = breedService;
    }

    [HttpGet("dropdown")]
    public async Task<IEnumerable<DropdownData>> GetAllBreedsForDropdown()
    {
        return await _breedService.GetAllBreedsForDropdown();
    }
}