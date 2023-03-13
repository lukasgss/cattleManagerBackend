using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Validation;
using CattleManager.Application.Application.Validation.Cattle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/cattle")]
public class CattleController : ControllerBase
{
    private readonly ICattleService _cattleService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public CattleController(ICattleService cattleService, IUserAuthorizationService userAuthorizationService)
    {
        _cattleService = cattleService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetAllCattlesFromOwner()
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.GetAllCattlesFromOwner(new Guid(userId));
        return Ok(cattle);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CattleResponse>> GetCattleById(Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        return await _cattleService.GetCattleById(id, new Guid(userId));
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetCattleByName(string name)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.GetCattleByNameAsync(name, new Guid(userId));
        return Ok(cattle);
    }

    [HttpPost]
    public async Task<ActionResult<CattleResponse>> CreateNewCattle(CattleRequest cattleRequest)
    {
        CreateCattleValidator validator = new();
        var validationResult = validator.Validate(cattleRequest);
        if (!validationResult.IsValid)
        {
            var modelStateDictionary = ValidationErrors.GenerateModelStateDictionary(validationResult);
            return ValidationProblem(modelStateDictionary);
        }

        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.CreateCattle(cattleRequest, new Guid(userId));
        return Ok(cattle);
    }
}