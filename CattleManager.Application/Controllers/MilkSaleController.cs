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
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<MilkSaleResponse> milkSales = await _milkSaleService.GetAllMilkSalesAsync(userId);
        return Ok(milkSales);
    }

    [HttpGet("{id:guid}", Name = "GetMilkSaleById")]
    public async Task<ActionResult<MilkSaleResponse>> GetMilkSaleById(Guid milkSaleId)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.GetMilkSaleByIdAsync(milkSaleId, userId);
        return Ok(milkSale);
    }

    [HttpGet("average/income")]
    public async Task<ActionResult<AverageOfEntity>> GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(int month, int year)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        AverageOfEntity averageTotalIncome =
            await _milkSaleService.GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(userId, month, year);
        return Ok(averageTotalIncome);
    }

    [HttpGet("price-history")]
    public async Task<ActionResult<IEnumerable<MilkPriceHistory>>> GetMilkPriceHistory()
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        IEnumerable<MilkPriceHistory> milkPriceHistory = await _milkSaleService.GetHistoryOfMilkPrices(userId);
        return Ok(milkPriceHistory);
    }

    [HttpGet("total-revenue")]
    public async Task<ActionResult<IEnumerable<DataInMonth<decimal>>>> GetTotalRevenueInPreviousMonths(int previousMonths)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        var totalRevenueInPreviousMonths = await _milkSaleService.GetMilkSalesTotalRevenueInPreviousMonths(previousMonths, userId);
        return Ok(totalRevenueInPreviousMonths);
    }

    [HttpPost]
    public async Task<ActionResult<MilkSaleResponse>> CreateMilkSale(CreateMilkSale createMilkSale)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.CreateMilkSaleAsync(createMilkSale, userId);
        return new CreatedAtRouteResult(nameof(GetMilkSaleById), new { milkSale.Id }, milkSale);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MilkSaleResponse>> EditMilkSale(EditMilkSale editMilkSale, Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        MilkSaleResponse milkSale = await _milkSaleService.EditMilkSaleAsync(editMilkSale, userId, id);
        return Ok(milkSale);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMilkSale(Guid id)
    {
        Guid userId = _userAuthorizationService.GetUserIdFromJwtToken(User);

        await _milkSaleService.DeleteMilkSaleAsync(id, userId);
        return Ok();
    }
}