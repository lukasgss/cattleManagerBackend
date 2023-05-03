using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Conceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/conceptions")]
public class ConceptionController : ControllerBase
{
    private readonly IConceptionService _conceptionService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public ConceptionController(IConceptionService conceptionService, IUserAuthorizationService userAuthorizationService)
    {
        _conceptionService = conceptionService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("{id:guid}", Name = "GetConceptionById")]
    public async Task<ActionResult<ICollection<ConceptionResponse>>> GetConceptionById(Guid id)
    {
        ConceptionResponse conception = await _conceptionService.GetConceptionByIdAsync(id);
        return Ok(conception);
    }

    [HttpGet("cattle/{id:guid}")]
    public async Task<ActionResult<ICollection<ConceptionResponse>>> GetAllConceptionsFromCattleByCattleId(Guid id, int page = 1)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        PaginatedConceptionResponse conceptionsFromCattle =
            await _conceptionService.GetAllConceptionsFromCattleAsync(id, userId, page);
        return Ok(conceptionsFromCattle);
    }

    [HttpPost]
    public async Task<ActionResult<ConceptionResponse>> CreateConception(CreateConceptionRequest conceptionRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        ConceptionResponse createdConception =
            await _conceptionService.CreateConceptionAsync(conceptionRequest, userId);
        return new CreatedAtRouteResult(nameof(GetConceptionById), new { id = createdConception.Id }, createdConception);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ConceptionResponse>> EditConception(EditConceptionRequest conceptionRequest, Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        ConceptionResponse conceptionResponse = await _conceptionService.EditConceptionAsync(conceptionRequest, userId, id);
        return Ok(conceptionResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteConception(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _conceptionService.DeleteConceptionAsync(id, userId);
        return Ok();
    }
}