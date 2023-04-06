using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/dropdown")]
public class DropdownDataController : ControllerBase
{
    private readonly IDropdownDataService _dropdownDataService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public DropdownDataController(IDropdownDataService dropdownDataService, IUserAuthorizationService userAuthorizationService)
    {
        _dropdownDataService = dropdownDataService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("cattle/male")]
    public async Task<ActionResult<DropdownDataResponse>> GetMaleCattleByName(string name)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var maleCattleByName = await _dropdownDataService.GetMaleCattleByName(name!, new Guid(userId));
        return Ok(maleCattleByName);
    }
}