using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Common.Interfaces.Entities.Farms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("/api/farms")]
public class FarmController : ControllerBase
{
    private readonly IFarmService _farmService;
    private readonly IUserAuthorizationService _userAuthorizationService;
    public FarmController(IFarmService farmService, IUserAuthorizationService userAuthorizationService)
    {
        _farmService = farmService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("{farmId:guid}", Name = "GetById")]
    public async Task<ActionResult<FarmResponse>> GetById(Guid farmId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        FarmResponse farm = await _farmService.GetFarmByIdAsync(userId, farmId);
        return Ok(farm);
    }

    [HttpPost]
    public async Task<ActionResult<FarmResponse>> Create(CreateFarmRequest createFarmRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        FarmResponse createdFarm = await _farmService.CreateFarmAsync(userId, createFarmRequest);
        return new CreatedAtRouteResult(nameof(GetById), new { farmId = createdFarm.Id }, createdFarm);
    }

    [HttpPut("{farmId:guid}")]
    public async Task<ActionResult<FarmResponse>> Edit(Guid farmId, EditFarmRequest editFarmRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        FarmResponse editedFarm = await _farmService.EditFarmAsync(userId, editFarmRequest, farmId);
        return Ok(editedFarm);
    }

    [HttpDelete("{farmId:guid}")]
    public async Task<ActionResult> Delete(Guid farmId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _farmService.DeleteFarmAsync(farmId, userId);
        return Ok();
    }
}