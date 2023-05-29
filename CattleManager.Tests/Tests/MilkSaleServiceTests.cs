using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DashboardHelper;
using CattleManager.Application.Application.Common.Interfaces.Entities.MilkSales;
using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using CattleManager.Application.Application.Common.Interfaces.GuidProvider;
using CattleManager.Application.Application.Common.Interfaces.InCommon;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.Entities;
using CattleManager.Application.Domain.Entities;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class MilkSaleServiceTests
{
    private readonly IMilkSaleService _sut;
    private readonly IMilkSaleRepository _milkSaleRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly IGuidProvider _guidProvider;
    private readonly IServiceValidations _serviceValidationsMock;
    private readonly IDashboardHelper _dashboardHelperMock;
    private static readonly Guid _milkSaleId = Guid.NewGuid();
    private static readonly Guid _userId = Guid.NewGuid();

    public MilkSaleServiceTests()
    {
        _milkSaleRepositoryMock = A.Fake<IMilkSaleRepository>();
        _userRepositoryMock = A.Fake<IUserRepository>();
        _mapperMock = A.Fake<IMapper>();
        _guidProvider = A.Fake<IGuidProvider>();
        _serviceValidationsMock = A.Fake<IServiceValidations>();
        _dashboardHelperMock = A.Fake<IDashboardHelper>();
        _sut = new MilkSaleService(
            _milkSaleRepositoryMock,
            _mapperMock,
            _userRepositoryMock,
            _guidProvider,
            _serviceValidationsMock,
            _dashboardHelperMock);
    }

    [Fact]
    public async Task Get_All_Milk_Sales_Returns_All_Milk_Sales()
    {
        MilkSale milkSale = GenerateMilkSale();
        IEnumerable<MilkSaleResponse> expectedMilkSaleResponse = new List<MilkSaleResponse>() { GenerateMilkSaleResponseFromMilkSale(milkSale) };
        A.CallTo(() => _milkSaleRepositoryMock.GetAllMilkSalesAsync(_userId)).Returns(new List<MilkSale>() { milkSale });

        IEnumerable<MilkSaleResponse> milkSaleResponse = await _sut.GetAllMilkSalesAsync(_userId);

        Assert.Equivalent(expectedMilkSaleResponse, milkSaleResponse);
    }

    [Fact]
    public async Task Get_Milk_Sale_By_Non_Existent_Id_Throws_NotFoundException()
    {
        MilkSale? nullMilkSale = null;
        A.CallTo(() => _milkSaleRepositoryMock.GetMilkSaleById(_milkSaleId, _userId, true)).Returns(nullMilkSale);

        async Task result() => await _sut.GetMilkSaleByIdAsync(_milkSaleId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Venda de leite com o id especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Get_Milk_Sale_By_Id_Returns_Milk_Sale()
    {
        MilkSale? milkSale = GenerateMilkSale();
        A.CallTo(() => _milkSaleRepositoryMock.GetMilkSaleById(_milkSaleId, _userId, true)).Returns(milkSale);
        MilkSaleResponse expectedMilkSaleResponse = GenerateMilkSaleResponseFromMilkSale(milkSale);

        MilkSaleResponse milkSaleResponse = await _sut.GetMilkSaleByIdAsync(_milkSaleId, _userId);

        Assert.Equivalent(expectedMilkSaleResponse, milkSaleResponse);
    }

    [Fact]
    public async Task Get_Milk_Sales_Average_Total_Income_In_Specific_Date_Returns_Average_Total_Income()
    {
        const int month = 2;
        const int year = 2023;
        AverageOfEntity expectedAverageTotalIncome = new()
        {
            Average = 8000,
            Quantity = 16
        };

        A.CallTo(() => _milkSaleRepositoryMock.GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(_userId, month, year))
            .Returns(expectedAverageTotalIncome);

        AverageOfEntity averageTotalIncome = await _sut.GetMilkSalesAverageRevenuePerSaleInSecificMonthAsync(_userId, month, year);

        Assert.Equivalent(expectedAverageTotalIncome, averageTotalIncome);
    }

    [Fact]
    public async Task Get_History_Of_Milk_Prices_Return_History_Of_Prices()
    {
        IEnumerable<MilkPriceHistory> expectedMilkPriceHistory = GenerateListOfMilkPriceHistory();
        A.CallTo(() => _milkSaleRepositoryMock.GetMilkPriceHistoryAsync(_userId)).Returns(expectedMilkPriceHistory);

        IEnumerable<MilkPriceHistory> milkPriceHistory = await _sut.GetHistoryOfMilkPrices(_userId);

        Assert.Equivalent(expectedMilkPriceHistory, milkPriceHistory);
    }

    [Fact]
    public async Task Get_Milk_Sales_Total_Revenue_In_Previous_Months_With_Month_Less_Than_1_Throws_BadRequestException()
    {
        const int previousMonths = 0;

        async Task result() => await _sut.GetMilkSalesTotalRevenueInPreviousMonths(previousMonths, _userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Valor dos meses anteriores deve ser maior ou igual a 1.", exception.Message);
    }

    [Fact]
    public async Task Get_Milk_Sales_Total_Revenue_Returns_Total_Revenue()
    {
        const int previousMonths = 2;
        IEnumerable<IEnumerable<MilkSaleByMonth>> milkSalesData = GenerateMilkSalesData();
        A.CallTo(() => _milkSaleRepositoryMock.GetTotalRevenueInPreviousMonths(previousMonths, _userId)).Returns(milkSalesData);
        IEnumerable<DataInMonth<decimal>> expectedMilkSalesTotalRevenueByMonth = GenerateMilkSalesTotalRevenueByMonth();
        A.CallTo(() => _dashboardHelperMock.FillTotalSumOfValueByMonths(milkSalesData, previousMonths)).Returns(expectedMilkSalesTotalRevenueByMonth);

        var milkSalesTotalRevenueByMonth = await _sut.GetMilkSalesTotalRevenueInPreviousMonths(previousMonths, _userId);

        Assert.Equivalent(expectedMilkSalesTotalRevenueByMonth, milkSalesTotalRevenueByMonth);
    }

    [Fact]
    public async Task Create_Milk_Sale_With_Non_Existent_Owner_Throws_BadRequestException()
    {
        User? nullMilkSaleOwner = null;
        MilkSale milkSale = GenerateMilkSale();
        CreateMilkSale createMilkSale = GenerateCreateMilkSaleFromMilkSale(milkSale);
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_userId)).Returns(nullMilkSaleOwner);

        async Task result() => await _sut.CreateMilkSaleAsync(createMilkSale, _userId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Dono da venda especificado não existe.", exception.Message);
    }

    [Fact]
    public async Task Create_Milk_Sale_Returns_Created_Milk_Sale()
    {
        User milkSaleOwner = GenerateUser();
        A.CallTo(() => _userRepositoryMock.GetByIdAsync(_userId)).Returns(milkSaleOwner);
        A.CallTo(() => _guidProvider.NewGuid()).Returns(_milkSaleId);
        MilkSale milkSale = GenerateMilkSale();
        CreateMilkSale createMilkSale = GenerateCreateMilkSaleFromMilkSale(milkSale);
        MilkSaleResponse expectedMilkSaleResponse = GenerateMilkSaleResponseFromMilkSale(milkSale);

        MilkSaleResponse milkSaleResponse = await _sut.CreateMilkSaleAsync(createMilkSale, _userId);

        Assert.Equivalent(expectedMilkSaleResponse, milkSaleResponse);
    }

    [Fact]
    public async Task Edit_Milk_Sale_With_Different_Route_Than_Id_Throws_BadRequestException()
    {
        Guid routeId = Guid.NewGuid();
        MilkSale milkSale = GenerateMilkSale();
        EditMilkSale editMilkSale = GenerateEditMilkSaleFromMilkSale(milkSale);

        async Task result() => await _sut.EditMilkSaleAsync(editMilkSale, _userId, routeId);

        var exception = await Assert.ThrowsAsync<BadRequestException>(result);
        Assert.Equal("Rota não coincide com o id especificado.", exception.Message);
    }

    [Fact]
    public async Task Edit_Non_Existent_Milk_Sale_Throws_NotFoundException()
    {
        MilkSale milkSale = GenerateMilkSale();
        EditMilkSale editMilkSale = GenerateEditMilkSaleFromMilkSale(milkSale);
        A.CallTo(() => _milkSaleRepositoryMock.GetMilkSaleById(_milkSaleId, _userId, false)).Returns(milkSale);
        A.CallTo(() => _mapperMock.Map<MilkSale>(editMilkSale)).Returns(milkSale);
        MilkSaleResponse expectedMilkSaleResponse = GenerateMilkSaleResponseFromMilkSale(milkSale);

        MilkSaleResponse milkSaleResponse = await _sut.EditMilkSaleAsync(editMilkSale, _userId, _milkSaleId);

        Assert.Equivalent(expectedMilkSaleResponse, milkSaleResponse);
    }

    [Fact]
    public async Task Delete_Non_Existent_Milk_Sale_Throws_NotFoundException()
    {
        MilkSale? nullMilkSale = null;
        A.CallTo(() => _milkSaleRepositoryMock.GetMilkSaleById(_milkSaleId, _userId, true)).Returns(nullMilkSale);

        async Task result() => await _sut.DeleteMilkSaleAsync(_milkSaleId, _userId);

        var exception = await Assert.ThrowsAsync<NotFoundException>(result);
        Assert.Equal("Venda de leite com o id especificado não existe.", exception.Message);
    }

    private static MilkSale GenerateMilkSale()
    {
        return new MilkSale()
        {
            Id = _milkSaleId,
            MilkInLiters = 370,
            PricePerLiter = 2.43m,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Owner = new User() { Id = _userId }
        };
    }

    private static MilkSaleResponse GenerateMilkSaleResponseFromMilkSale(MilkSale milkSale)
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

    private static CreateMilkSale GenerateCreateMilkSaleFromMilkSale(MilkSale milkSale)
    {
        return new CreateMilkSale(
            MilkInLiters: milkSale.MilkInLiters,
            PricePerLiter: milkSale.PricePerLiter,
            Date: milkSale.Date);
    }

    private static User GenerateUser()
    {
        return new User()
        {
            Id = _userId,
        };
    }

    private static EditMilkSale GenerateEditMilkSaleFromMilkSale(MilkSale milkSale)
    {
        return new EditMilkSale(
            Id: milkSale.Id,
            MilkInLiters: milkSale.MilkInLiters,
            PricePerLiter: milkSale.PricePerLiter,
            Date: milkSale.Date
        );
    }

    private static IEnumerable<MilkPriceHistory> GenerateListOfMilkPriceHistory()
    {
        return new List<MilkPriceHistory>()
        {
            new MilkPriceHistory()
            {
                From = new DateOnly(2020, 08, 04),
                To = new DateOnly(2021, 06, 02),
                Price = 2.32m
            },
            new MilkPriceHistory()
            {
                From = new DateOnly(2021, 06, 08),
                To = null,
                Price = 2.46m
            },
        };
    }

    private static IEnumerable<IEnumerable<MilkSaleByMonth>> GenerateMilkSalesData()
    {
        return new List<List<MilkSaleByMonth>>()
        {
            new List<MilkSaleByMonth>()
            {
                new MilkSaleByMonth()
                {
                    Date = DateOnly.FromDateTime(new DateTime(2020, 1, 1)),
                    Value = 0
                },
            },
            new List<MilkSaleByMonth>()
            {
                new MilkSaleByMonth()
                {
                    Date = DateOnly.FromDateTime(new DateTime(2020, 2, 1)),
                    Value = 0
                },
            }
        };
    }

    private static IEnumerable<DataInMonth<decimal>> GenerateMilkSalesTotalRevenueByMonth()
    {
        return new List<DataInMonth<decimal>>()
        {
            new DataInMonth<decimal>()
            {
                Month = "jan.",
                Value = 0
            },
            new DataInMonth<decimal>()
            {
                Month = "fev.",
                Value = 0
            },
        };
    }
}