using FluentValidation;

namespace CattleManager.Application.Application.Validation.CustomValidations;

public static class DateValidator
{
    public static IRuleBuilderOptions<T, DateOnly?> DateShouldNotBeInTheFuture<T>(
        this IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        var todaysDate = DateOnly.FromDateTime(DateTime.Now);

        return ruleBuilder.Must(date => date <= todaysDate).When(date => date is not null).WithMessage("Data inserida não pode ser maior que a data atual.");
    }

    public static IRuleBuilderOptions<T, int> YearShouldNotBeGreaterThanCurrent<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        var currentYear = DateTime.Now.Year;

        return ruleBuilder.Must(year => year <= currentYear).WithMessage("Ano inserido não pode ser maior que o ano atual.");
    }

    public static bool YearInDateOfBirthShouldBeEqualToBirthYear(DateOnly? dateOfBirth, int yearOfBirth)
    {
        if (dateOfBirth is null)
            return true;

        var birthYear = dateOfBirth?.Year;

        return yearOfBirth == birthYear;
    }
}