using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.CommonValidations;
using CattleManager.Application.Domain.Entities;

namespace CattleManager.Application.Application.Services.Entities;

public class MilkProductionService : IMilkProductionService
{
    private readonly IMilkProductionRepository _milkProductionRepository;
    private readonly ICattleRepository _cattleRepository;
    private readonly IMapper _mapper;
    private readonly IServiceValidations _serviceValidations;
    private readonly IDashboardHelper _dashboardHelper;

    public MilkProductionService(
        IMilkProductionRepository milkProductionRepository,
        ICattleRepository cattleRepository,
        IMapper mapper,
        IServiceValidations serviceValidations,
        IDashboardHelper dashboardHelper)
    {
        _milkProductionRepository = milkProductionRepository;
        _cattleRepository = cattleRepository;
        _mapper = mapper;
        _serviceValidations = serviceValidations;
        _dashboardHelper = dashboardHelper;
    }

    public async Task<PaginatedMilkProductionResponse> GetAllMilkProductionsAsync(Guid userId, int page)
    {
        double amountOfPages = _milkProductionRepository.GetAmountOfPages(userId);
        if ((page > amountOfPages && amountOfPages > 1) || page < 1)
            throw new BadRequestException($"Resultado possui {amountOfPages} página(s), insira um valor entre 1 e o número de páginas.");

        IEnumerable<MilkProduction> milkProductions = await _milkProductionRepository.GetAllMilkProductionsAsync(userId, page);
        List<MilkProductionResponse> milkProductionsResponse = PopulateMilkProductionResponseList(milkProductions);

        return new PaginatedMilkProductionResponse(milkProductionsResponse, page, amountOfPages);
    }
    public async Task<MilkProductionResponse> GetMilkProductionByIdAsync(Guid milkProductionId, Guid userId)
    {
        var milkProduction = await _milkProductionRepository.GetMilkProductionByIdAsync(milkProductionId, userId);
        if (milkProduction is null)
            throw new NotFoundException("Não foi possível encontrar produção de leite com o id especificado.");

        return ConvertToMilkProductionResponse(milkProduction);
    }

    public async Task<PaginatedMilkProductionResponse> GetAllMilkProductionsFromCattleAsync(Guid cattleId, Guid userId, int page)
    {
        var cattle = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não foi encontrado.");

        double amountOfPages = _milkProductionRepository.GetAmountOfPages(cattleId, userId);
        if ((page > amountOfPages && amountOfPages > 1) || page < 1)
            throw new BadRequestException($"Resultado possui {amountOfPages} página(s), insira um valor entre 1 e o número de páginas.");

        var milkProductions = await _milkProductionRepository.GetMilkProductionsFromCattleAsync(cattleId, userId, page);

        List<MilkProductionResponse> milkProductionsResponse = PopulateMilkProductionResponseList(milkProductions);
        return new PaginatedMilkProductionResponse(milkProductionsResponse, page, amountOfPages);
    }

    public async Task<AverageOfEntity> GetAverageMilkProductionFromAllCattleAsync(Guid userId, int month, int year)
    {
        ServiceValidations.ValidateMonth(month);
        _serviceValidations.ValidateDate(month, year);

        return await _milkProductionRepository.GetAverageMilkProductionFromAllCattleAsync(userId, month, year);
    }

    public async Task<AverageMilkProduction> GetAverageMilkProductionFromCattleAsync(Guid cattleId, Guid userId, int month, int year)
    {
        ServiceValidations.ValidateMonth(month);
        _serviceValidations.ValidateDate(month, year);

        Cattle? cattle = await _cattleRepository.GetCattleById(cattleId, userId, false);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não existe.");

        return await _milkProductionRepository.GetAverageMilkProductionFromCattleAsync(cattleId, userId, month, year);
    }

    public async Task<IEnumerable<DataInMonth<decimal>>> GetAmountOfMilkProductionLastMonthsAsync(int previousMonths, Guid userId)
    {
        if (previousMonths <= 0)
            throw new BadRequestException("Valor dos meses anteriores deve ser maior ou igual a 1.");

        var milkProductionLastMonths = await _milkProductionRepository.GetTotalMilkProductionLastMonthsAsync(previousMonths, userId);

        return _dashboardHelper.FillTotalSumOfValueByMonths(milkProductionLastMonths, previousMonths);
    }

    public async Task<CreateMilkProductionResponse> CreateMilkProductionAsync(MilkProductionRequest milkProductionRequest, Guid userId)
    {
        var cattleOfMilkProduction = await _cattleRepository.GetCattleById(milkProductionRequest.CattleId, userId, false);
        if (cattleOfMilkProduction is null)
            throw new NotFoundException("Usuário não possui um animal com o id especificado.");

        MilkProduction milkProduction = _mapper.Map<MilkProduction>(milkProductionRequest);
        _milkProductionRepository.Add(milkProduction);
        await _milkProductionRepository.CommitAsync();

        return new CreateMilkProductionResponse(
            Id: milkProduction.Id,
            MilkInLiters: milkProduction.MilkInLiters,
            PeriodOfDay: TranslatePeriodOfDay(milkProduction.PeriodOfDay),
            Date: milkProduction.Date.ToString("dd/MM/yyyy"),
            CattleId: milkProduction.CattleId
        );
    }

    public async Task<MilkProductionResponse> EditMilkProductionByIdAsync(EditMilkProductionRequest milkProductionRequest, Guid userId, Guid routeId)
    {
        if (routeId != milkProductionRequest.Id)
            throw new BadRequestException("Id da rota e o especificado da produção de leite não coincidem.");

        MilkProduction? milkProduction = await _milkProductionRepository.GetMilkProductionByIdAsync(milkProductionRequest.Id, userId);
        if (milkProduction is null)
            throw new NotFoundException("Produção de leite com o id especificado não foi encontrado.");

        Cattle? cattle = await _cattleRepository.GetByIdAsync(milkProductionRequest.CattleId);
        if (cattle is null)
            throw new NotFoundException("Animal com o id especificado não foi encontrado.");

        MilkProduction milkProductionToEdit = _mapper.Map<MilkProduction>(milkProductionRequest);
        _milkProductionRepository.Update(milkProductionToEdit);
        await _milkProductionRepository.CommitAsync();
        return _mapper.Map<MilkProductionResponse>(milkProductionToEdit);
    }

    public async Task DeleteMilkProductionByIdAsync(Guid milkProductionId, Guid userId)
    {
        MilkProduction? milkProduction = await _milkProductionRepository.GetMilkProductionByIdAsync(milkProductionId, userId, false);
        if (milkProduction is null)
            throw new NotFoundException("Produção de leite com o id especificado não foi encontrado.");

        _milkProductionRepository.Delete(milkProduction);
        await _milkProductionRepository.CommitAsync();
    }

    private static MilkProductionResponse ConvertToMilkProductionResponse(MilkProduction milkProduction)
    {
        return new MilkProductionResponse(
            Id: milkProduction.Id,
            CattleName: milkProduction.Cattle.Name,
            MilkInLiters: milkProduction.MilkInLiters,
            PeriodOfDay: TranslatePeriodOfDay(milkProduction.PeriodOfDay),
            Date: milkProduction.Date.ToString("dd/MM/yyyy"),
            CattleId: milkProduction.CattleId);
    }

    private static List<MilkProductionResponse> PopulateMilkProductionResponseList(IEnumerable<MilkProduction> milkProductions)
    {
        List<MilkProductionResponse> milkProductionsResponse = new();
        foreach (MilkProduction milkProduction in milkProductions)
        {
            milkProductionsResponse.Add(ConvertToMilkProductionResponse(milkProduction));
        }
        return milkProductionsResponse;
    }

    private static string TranslatePeriodOfDay(char periodOfDay)
    {
        return periodOfDay switch
        {
            'a' => "Tarde",
            'd' => "Dia inteiro",
            'm' => "Manhã",
            'n' => "Noite",
            _ => throw new Exception("Invalid period of day.")
        };
    }
}