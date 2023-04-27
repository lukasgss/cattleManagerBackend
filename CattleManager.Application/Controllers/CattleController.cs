using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Common;
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
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.GetAllCattleFromOwner(userId, page);
        return Ok(cattle);
    }

    [HttpGet("{id:guid}", Name = "GetCattleById")]
    public async Task<ActionResult<CattleResponse>> GetCattleById(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        return await _cattleService.GetCattleById(id, userId);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetCattleByName(string name)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.GetCattleByNameAsync(name, userId);
        return Ok(cattle);
    }

    [HttpGet("children/{id:guid}")]
    public async Task<ActionResult<IEnumerable<CattleResponse>>> GetAllChildrenFromCattle(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<CattleResponse> cattleChildren = await _cattleService.GetAllChildrenFromCattle(id, userId);
        return Ok(cattleChildren);
    }

    [HttpGet("dropdown/male")]
    public async Task<ActionResult<DropdownData>> GetMaleCattleByName(string name)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var maleCattleByName = await _cattleService.GetMaleCattleByName(name, userId);
        return Ok(maleCattleByName);
    }

    [HttpGet("dropdown/female")]
    public async Task<ActionResult<DropdownData>> GetFemaleCattleByNamer(string name)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var femaleCattleByName = await _cattleService.GetFemaleCattleByName(name, userId);
        return Ok(femaleCattleByName);
    }

    [HttpGet("lactation-period/amount")]
    public async Task<ActionResult<AmountOfEntity>> GetAmountOfCattleInLactationPeriod()
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AmountOfEntity amount = await _cattleService.GetAmountOfCattleInLactationPeriodAsync(userId);
        return Ok(amount);
    }

    [HttpGet("dry-period/amount")]
    public async Task<ActionResult<AmountOfEntity>> GetAmountOfCattleInDryPeriod()
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AmountOfEntity amount = await _cattleService.GetAmountOfCattleInDryPeriodAsync(userId);
        return Ok(amount);
    }

    [HttpGet("calving-intervals/{cattleId:guid}")]
    public async Task<ActionResult<IEnumerable<CalvingInterval>>> GetAllCalvingIntervalsFromCattle(Guid cattleId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<CalvingInterval> calvingIntervals =
            await _cattleService.GetAllCalvingIntervalsFromCattleAsync(cattleId, userId);

        return Ok(calvingIntervals);
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

        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.CreateCattle(cattleRequest, userId);
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

        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var cattle = await _cattleService.EditCattle(cattleRequest, userId, cattleId);
        return Ok(cattle);
    }

    [HttpDelete("{cattleId:guid}")]
    public async Task<ActionResult> DeleteCattle(Guid cattleId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _cattleService.DeleteCattle(cattleId, userId);
        return Ok();
    }
}