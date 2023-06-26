using CattleManager.Application.Application.Common.Exceptions;
using CattleManager.Application.Application.Common.Interfaces.DateTimeProvider;
using CattleManager.Application.Application.Common.Interfaces.ServiceValidations;

namespace CattleManager.Application.Application.Services.CommonValidations;

public class ServiceValidations : IServiceValidations
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ServiceValidations(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public static void ValidateMonth(int month)
    {
        if (month < 1 || month > 12)
            throw new BadRequestException("Mês deve ser entre 1 e 12.");
    }

    public void ValidateDate(int month, int year)
    {
        int currentMonth = _dateTimeProvider.Now().Month;
        int currentYear = _dateTimeProvider.Now().Year;

        if (year > currentYear || (month > currentMonth && year == currentYear))
            throw new BadRequestException("Data especificada deve ser menor ou igual à data atual.");
    }
}