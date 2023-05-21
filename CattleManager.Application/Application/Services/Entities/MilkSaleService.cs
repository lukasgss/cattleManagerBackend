using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.CommonValidations;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class MilkSaleService : IMilkSaleService
{
    private readonly IMilkSaleRepository _milkSaleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IGuidProvider _guidProvider;
    private readonly IServiceValidations _serviceValidations;

    public MilkSaleService(
        IMilkSaleRepository milkSaleRepository,
        IMapper mapper,
        IUserRepository userRepository,
        IGuidProvider guidProvider,
        IServiceValidations serviceValidations)
    {
        _milkSaleRepository = milkSaleRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _guidProvider = guidProvider;
        _serviceValidations = serviceValidations;
    }

    public async Task<IEnumerable<MilkSaleResponse>> GetAllMilkSalesAsync(Guid userId)
    {
        IEnumerable<MilkSale> milkSales = await _milkSaleRepository.GetAllMilkSalesAsync(userId);
        List<MilkSaleResponse> milkSalesResponse = new();
        foreach (MilkSale milkSale in milkSales)
        {
            milkSalesResponse.Add(GenerateMilkSaleResponse(milkSale));
        }

        return milkSalesResponse;
    }

    public async Task<MilkSaleResponse> GetMilkSaleByIdAsync(Guid milkSaleId, Guid userId)
    {
        MilkSale? milkSale = await _milkSaleRepository.GetMilkSaleById(milkSaleId, userId);
        if (milkSale is null)
            throw new NotFoundException("Venda de leite com o id especificado não existe.");

        return new MilkSaleResponse()
        {
            Id = milkSale.Id,
            MilkInLiters = milkSale.MilkInLiters,
            PricePerLiter = milkSale.PricePerLiter,
            TotalPrice = milkSale.MilkInLiters * milkSale.PricePerLiter,
            Date = milkSale.Date
        };
    }

    public async Task<IEnumerable<MilkPriceHistory>> GetHistoryOfMilkPrices(Guid userId)
    {
        return await _milkSaleRepository.GetMilkPriceHistoryAsync(userId);
    }

    public async Task<AverageOfEntity> GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(Guid userId, int month, int year)
    {
        ServiceValidations.ValidateMonth(month);
        _serviceValidations.ValidateDate(month, year);

        return await _milkSaleRepository.GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(userId, month, year);
    }

    public async Task<MilkSaleResponse> CreateMilkSaleAsync(CreateMilkSale createMilkSale, Guid userId)
    {
        User? milkSaleOwner = await _userRepository.GetByIdAsync(userId);
        if (milkSaleOwner is null)
            throw new BadRequestException("Dono da venda especificado não existe.");

        MilkSale milkSale = new()
        {
            Id = _guidProvider.NewGuid(),
            MilkInLiters = createMilkSale.MilkInLiters,
            PricePerLiter = createMilkSale.PricePerLiter,
            Date = createMilkSale.Date,
            Owner = milkSaleOwner
        };

        _milkSaleRepository.Add(milkSale);
        await _milkSaleRepository.CommitAsync();

        return new MilkSaleResponse()
        {
            Id = milkSale.Id,
            MilkInLiters = milkSale.MilkInLiters,
            PricePerLiter = milkSale.PricePerLiter,
            TotalPrice = milkSale.MilkInLiters * milkSale.PricePerLiter,
            Date = milkSale.Date
        };
    }

    public async Task<MilkSaleResponse> EditMilkSaleAsync(EditMilkSale editMilkSale, Guid userId, Guid routeId)
    {
        if (routeId != editMilkSale.Id)
            throw new BadRequestException("Rota não coincide com o id especificado.");

        MilkSale? milkSaleToEdit = await _milkSaleRepository.GetMilkSaleById(editMilkSale.Id, userId, trackChanges: false);
        if (milkSaleToEdit is null)
            throw new NotFoundException("Venda de leite com o id especificado não existe.");

        MilkSale editedMilkSale = _mapper.Map<MilkSale>(editMilkSale);
        _milkSaleRepository.Update(editedMilkSale);
        await _milkSaleRepository.CommitAsync();

        return new MilkSaleResponse()
        {
            Id = editedMilkSale.Id,
            MilkInLiters = editedMilkSale.MilkInLiters,
            PricePerLiter = editedMilkSale.PricePerLiter,
            TotalPrice = editedMilkSale.MilkInLiters * editedMilkSale.PricePerLiter,
            Date = editedMilkSale.Date
        };
    }

    public async Task DeleteMilkSaleAsync(Guid milkSaleId, Guid userId)
    {
        MilkSale? milkSaleToDelete = await _milkSaleRepository.GetMilkSaleById(milkSaleId, userId);
        if (milkSaleToDelete is null)
            throw new NotFoundException("Venda de leite com o id especificado não existe.");

        _milkSaleRepository.Delete(milkSaleToDelete);
        await _milkSaleRepository.CommitAsync();
    }

    private static MilkSaleResponse GenerateMilkSaleResponse(MilkSale milkSale)
    {
        return new MilkSaleResponse()
        {
            Id = milkSale.Id,
            MilkInLiters = milkSale.MilkInLiters,
            PricePerLiter = milkSale.PricePerLiter,
            TotalPrice = milkSale.MilkInLiters * milkSale.PricePerLiter,
            Date = milkSale.Date
        };
    }
}