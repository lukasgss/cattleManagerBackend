using CattleManager.Application.Application.Common.Interfaces.Authorization;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CattleManager.Application.Controllers;

[Authorize]
[ApiController]
[Route("/api/milk-sales")]
public class MilkSaleController : ControllerBase
{
    private readonly IMilkSaleService _milkSaleService;
    private readonly IUserAuthorizationService _userAuthorizationService;

    public MilkSaleController(IMilkSaleService milkSaleService, IUserAuthorizationService userAuthorizationService)
    {
        _milkSaleService = milkSaleService;
        _userAuthorizationService = userAuthorizationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MilkSaleResponse>>> GetAllMilkSales()
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<MilkSaleResponse> milkSales = await _milkSaleService.GetAllMilkSalesAsync(new Guid(userId));
        return Ok(milkSales);
    }

    [HttpGet("{id:guid}", Name = "GetMilkSaleById")]
    public async Task<ActionResult<MilkSaleResponse>> GetMilkSaleById(Guid milkSaleId)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.GetMilkSaleByIdAsync(milkSaleId, new Guid(userId));
        return Ok(milkSale);
    }

    [HttpGet("average/income")]
    public async Task<ActionResult<AverageOfEntity>> GetMilkSaleAverageTotalIncomeInSpecificMonth(int month, int year)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AverageOfEntity averageTotalIncome =
            await _milkSaleService.GetMilkSalesAverageTotalIncomeInSpecificMonthAsync(new Guid(userId), month, year);
        return Ok(averageTotalIncome);
    }

    [HttpPost]
    public async Task<ActionResult<MilkSaleResponse>> CreateMilkSale(CreateMilkSale createMilkSale)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.CreateMilkSaleAsync(createMilkSale, new Guid(userId));
        return new CreatedAtRouteResult(nameof(GetMilkSaleById), new { milkSale.Id }, milkSale);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MilkSaleResponse>> EditMilkSale(EditMilkSale editMilkSale, Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.EditMilkSaleAsync(editMilkSale, new Guid(userId), id);
        return Ok(milkSale);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMilkSale(Guid id)
    {
        string userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _milkSaleService.DeleteMilkSaleAsync(id, new Guid(userId));
        return Ok();
    }
}