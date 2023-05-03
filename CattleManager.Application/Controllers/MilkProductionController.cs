using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
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
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        return await _milkProductionService.GetMilkProductionByIdAsync(id, userId);
    }

    [Route("cattle/{cattleId:guid}")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MilkProductionResponse>>> GetMilkProductionsFromCattle(Guid cattleId, int page = 1)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        PaginatedMilkProductionResponse milkProductions =
            await _milkProductionService.GetAllMilkProductionsFromCattleAsync(cattleId, userId, page);
        return Ok(milkProductions);
    }

    [HttpGet("average")]
    public async Task<ActionResult<AverageOfEntity>> GetAverageMilkProductionFromAllCattle(int month, int year)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AverageOfEntity averageMilkProduction =
            await _milkProductionService.GetAverageMilkProductionFromAllCattleAsync(userId, month, year);

        return Ok(averageMilkProduction);
    }

    [HttpGet("average/{cattleId:guid}")]
    public async Task<ActionResult<AverageMilkProduction>> GetAverageMilkProductionFromCattle(Guid cattleId, int month, int year)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AverageMilkProduction averageMilkProduction =
            await _milkProductionService.GetAverageMilkProductionFromCattleAsync(cattleId, userId, month, year);

        return Ok(averageMilkProduction);
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

        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkProductionResponse milkProduction =
            await _milkProductionService.CreateMilkProductionAsync(milkProductionRequest, userId);
        return new CreatedAtRouteResult(nameof(GetMilkProductionById), new { milkProduction.Id }, milkProduction);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MilkProductionResponse>> EditMilkProduction(Guid id, EditMilkProductionRequest milkProduction)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkProductionResponse editedMilkProduction =
            await _milkProductionService.EditMilkProductionByIdAsync(milkProduction, userId, id);
        return Ok(editedMilkProduction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMilkProduction(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _milkProductionService.DeleteMilkProductionByIdAsync(id, userId);
        return Ok();
    }
}