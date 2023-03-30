using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Validation;
using CattleManager.Application.Application.Validation.MilkProduction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/milk-productions")]
public class MilkProductionController : ControllerBase
{
    private readonly IMilkProductionService _milkProductionService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public MilkProductionController(IMilkProductionService milkProductionService,
        IUserAuthorizationService userAuthorizationService)
    {
        _milkProductionService = milkProductionService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("{id:guid}", Name = "GetMilkProductionById")]
    public async Task<MilkProductionResponse> GetMilkProductionById(Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        return await _milkProductionService.GetMilkProductionByIdAsync(id, new Guid(userId));
    }

    [Route("cattle/{cattleId:guid}")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MilkProductionResponse>>> GetMilkProductionsFromCattle(Guid cattleId, int page = 1)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);
        var milkProductions = await _milkProductionService.GetAllMilkProductionsFromCattleAsync(cattleId, new Guid(userId), page);
        return Ok(milkProductions);
    }

    [HttpPost]
    public async Task<ActionResult<MilkProductionResponse>> CreateMilkProduction(MilkProductionRequest milkProductionRequest)
    {
        CreateMilkProductionValidator validator = new();
        var validationResult = validator.Validate(milkProductionRequest);
        if (!validationResult.IsValid)
        {
            ModelStateDictionary modelStateDictionary = ValidationErrors.GenerateModelStateDictionary(validationResult);
            return ValidationProblem(modelStateDictionary);
        }

        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var milkProduction = await _milkProductionService.CreateMilkProductionAsync(milkProductionRequest, new Guid(userId));
        return new CreatedAtRouteResult(nameof(GetMilkProductionById), new { milkProduction.Id }, milkProduction);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MilkProductionResponse>> EditMilkProduction(Guid id, EditMilkProductionRequest milkProduction)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var editedMilkProduction = await _milkProductionService.EditMilkProductionByIdAsync(milkProduction, new Guid(userId), id);
        return Ok(editedMilkProduction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMilkProduction(Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _milkProductionService.DeleteMilkProductionByIdAsync(id, new Guid(userId));
        return Ok();
    }
}