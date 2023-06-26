using CattleManager.Application.Application.Common.Interfaces.Entities.MilkProductions;
using CattleManager.Application.Application.Validation.CustomValidations;
using FluentValidation;

namespace CattleManager.Application.Application.Validation.MilkProduction;

public class CreateMilkProductionValidator : AbstractValidator<MilkProductionRequest>
{
    public CreateMilkProductionValidator()
    {
        RuleFor(x => x.MilkInLiters).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(x => x.PeriodOfDay).NotEmpty().ShouldBeValidPeriodOfDay();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.CattleId).NotEmpty();
    }
}