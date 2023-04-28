using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.MedicalRecords;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/medical-records")]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public MedicalRecordController(IMedicalRecordService medicalRecordService, IUserAuthorizationService userAuthorizationService)
    {
        _medicalRecordService = medicalRecordService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet("{id:guid}", Name = "GetMedicalRecordById")]
    public async Task<ActionResult<MedicalRecordResponse>> GetMedicalRecordById(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MedicalRecordResponse medicalRecord = await _medicalRecordService.GetMedicalRecordByIdAsync(id, userId);
        return Ok(medicalRecord);
    }

    [HttpPost]
    public async Task<ActionResult<MedicalRecordResponse>> CreateMedicalRecord(CreateMedicalRecord createMedicalRecord)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MedicalRecordResponse medicalRecord = await _medicalRecordService.CreateMedicalRecordAsync(createMedicalRecord, userId);
        return new CreatedAtRouteResult(nameof(GetMedicalRecordById), new { medicalRecord.Id }, medicalRecord);
    }
}