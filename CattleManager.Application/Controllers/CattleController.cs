using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.FrontendDropdownData;
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
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetAllCattlesFromOwner(int page = 1)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.GetAllCattleFromOwner(new Guid(userId), page);
        return Ok(cattle);
    }

    [HttpGet("{id:guid}", Name = "GetCattleById")]
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

    [HttpGet("children/{id:guid}")]
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetAllChildrenFromCattle(Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<CattleResponse> cattleChildren = await _cattleService.GetAllChildrenFromCattle(id, new Guid(userId));
        return Ok(cattleChildren);
    }

    [HttpGet("dropdown/male")]
    public async Task<ActionResult<DropdownData>> GetMaleCattleByName(string name)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var maleCattleByName = await _cattleService.GetMaleCattleByName(name, new Guid(userId));
        return Ok(maleCattleByName);
    }

    [HttpGet("dropdown/female")]
    public async Task<ActionResult<DropdownData>> GetFemaleCattleByNamer(string name)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var femaleCattleByName = await _cattleService.GetFemaleCattleByName(name, new Guid(userId));
        return Ok(femaleCattleByName);
    }

    [HttpGet("lactation-period/amount")]
    public async Task<ActionResult<AmountOfCattleInLactationPeriod>> GetAmountOfCattleInLactationPeriod()
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AmountOfCattleInLactationPeriod amount = await _cattleService.GetAmountOfCattleInLactationPeriodAsync(new Guid(userId));
        return Ok(amount);
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
        return new CreatedAtRouteResult(nameof(GetCattleById), new { id = cattle.Id }, cattle);
    }

    [HttpPut("{cattleId:guid}")]
    public async Task<ActionResult<CattleResponse>> EditCattle(EditCattleRequest cattleRequest, Guid cattleId)
    {
        EditCattleValidator validator = new();
        var validationResult = validator.Validate(cattleRequest);
        if (!validationResult.IsValid)
        {
            var modelStateDictionary = ValidationErrors.GenerateModelStateDictionary(validationResult);
            return ValidationProblem(modelStateDictionary);
        }

        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.EditCattle(cattleRequest, new Guid(userId), cattleId);
        return Ok(cattle);
    }

    [HttpDelete("{cattleId:guid}")]
    public async Task<ActionResult> DeleteCattle(Guid cattleId)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _cattleService.DeleteCattle(cattleId, new Guid(userId));
        return Ok();
    }
}