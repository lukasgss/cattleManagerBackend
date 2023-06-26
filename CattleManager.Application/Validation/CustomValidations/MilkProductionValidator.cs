using FluentValidation;

namespace CattleManager.Application.Application.Validation.CustomValidations;

public static class MilkProductionValidator
{
    private readonly static char[] _validPeriodsOfDay = new char[] { 'm', 'a', 'n', 'd' };

    public static IRuleBuilderOptions<T, string> ShouldBeValidPeriodOfDay<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(periodOfDay => Array.IndexOf(_validPeriodsOfDay, periodOfDay[0]) != -1 && periodOfDay.Length == 1)
        .WithMessage("Período do dia inválido. Valores válidos são: 'm', 'a', 'n' e 'd'.");
    }
}