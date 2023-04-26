using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.Vaccinations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/vaccinations")]
public class VaccinationController : ControllerBase
{
    private readonly IVaccinationService _vaccinationService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public VaccinationController(IVaccinationService vaccinationService, IUserAuthorizationService userAuthorizationService)
    {
        _vaccinationService = vaccinationService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("{cattleId:guid}")]
    public async Task<ActionResult<IEnumerable<VaccinationResponse>>> GetAllVaccinationsFromCattle(Guid cattleId, int page = 1)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var vaccinationsFromCattle = await _vaccinationService.GetAllVaccinationsFromCattleAsync(cattleId, userId, page);
        return Ok(vaccinationsFromCattle);
    }

    [HttpPost]
    public async Task<ActionResult<VaccinationResponse>> CreateVacination(CreateVaccinationRequest vaccinationRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var vaccination = await _vaccinationService.CreateVaccinationAsync(vaccinationRequest, userId);
        return Ok(vaccination);
    }

    [HttpPut("{vaccinationId:guid}")]
    public async Task<ActionResult<VaccinationResponse>> EditVaccination(Guid vaccinationId, EditVaccinationRequest vaccinationRequest)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var editedVaccination = await _vaccinationService.EditVaccinationAsync(vaccinationRequest, vaccinationId, userId);
        return Ok(editedVaccination);
    }

    [HttpDelete("{vaccinationId:guid}")]
    public async Task<ActionResult> DeleteVaccination(Guid vaccinationId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _vaccinationService.DeleteVaccinationAsync(vaccinationId, userId);
        return Ok();
    }
}