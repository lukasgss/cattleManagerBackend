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
    public async Task<ActionResult<IEnumerable<VaccinationResponse>>> GetAllVaccinationsFromCattle(Guid cattleId)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var vaccinationsFromCattle = await _vaccinationService.GetAllVaccinationsFromCattle(cattleId, new Guid(userId));
        return Ok(vaccinationsFromCattle);
    }

    [HttpPost]
    public async Task<ActionResult<VaccinationResponse>> CreateVacination(CreateVaccinationRequest vaccinationRequest)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var vaccination = await _vaccinationService.CreateVaccinationAsync(vaccinationRequest, new Guid(userId));
        return Ok(vaccination);
    }

    [HttpPut("{vaccinationId:guid}")]
    public async Task<ActionResult<VaccinationResponse>> EditVaccination(Guid vaccinationId, EditVaccinationRequest vaccinationRequest)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var editedVaccination = await _vaccinationService.EditVaccinationAsync(vaccinationRequest, vaccinationId, new Guid(userId));
        return Ok(editedVaccination);
    }

    [HttpDelete("{vaccinationId:guid}")]
    public async Task<ActionResult> DeleteVaccination(Guid vaccinationId)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _vaccinationService.DeleteVaccinationAsync(vaccinationId, new Guid(userId));
        return Ok();
    }
}