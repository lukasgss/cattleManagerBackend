using System;
using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;
using CattleManager.Application.Application.Services.CommonValidations;
using FakeItEasy;
using Xunit;

namespace CattleManager.Tests.Tests;

public class ServiceValidationsTests
{
    private readonly IServiceValidations _sut;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    public ServiceValidationsTests()
    {
        _dateTimeProviderMock = A.Fake<IDateTimeProvider>();
        _sut = new ServiceValidations(_dateTimeProviderMock);
    }

    [Fact]
    public void Month_Value_Lower_Than_1_Throws_BadRequestException()
    {
        const int smallerMonth = 0;

        void result() => _sut.ValidateMonth(smallerMonth);

        var exception = Assert.Throws<BadRequestException>(result);
        Assert.Equal("Mês deve ser entre 1 e 12.", exception.Message);
    }

    [Fact]
    public void Month_Value_Greater_Than_12_Throws_BadRequestException()
    {
        const int biggerMonth = 14;

        void result() => _sut.ValidateMonth(biggerMonth);

        var exception = Assert.Throws<BadRequestException>(result);
        Assert.Equal("Mês deve ser entre 1 e 12.", exception.Message);
    }

    [Fact]
    public void Year_Greater_Than_Current_Year_Throws_BadRequestException()
    {
        const int currentYear = 2023;
        const int futureYear = 5000;
        DateTime currentDate = new(currentYear, 1, 1);
        A.CallTo(() => _dateTimeProviderMock.Now()).Returns(currentDate);

        void result() => _sut.ValidateDate(1, futureYear);

        var exception = Assert.Throws<BadRequestException>(result);
        Assert.Equal("Data especificada deve ser menor ou igual à data atual.", exception.Message);
    }

    [Fact]
    public void Month_Greater_Than_Current_Month_And_Year_Equal_To_Current_Year_Throws_BadRequestException()
    {
        const int currentYear = 2023;
        const int currentMonth = 1;
        const int futureMonth = 12;
        DateTime currentDate = new(currentYear, currentMonth, 1);
        A.CallTo(() => _dateTimeProviderMock.Now()).Returns(currentDate);

        void result() => _sut.ValidateDate(futureMonth, currentYear);

        var exception = Assert.Throws<BadRequestException>(result);
        Assert.Equal("Data especificada deve ser menor ou igual à data atual.", exception.Message);
    }
}