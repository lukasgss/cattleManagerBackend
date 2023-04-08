using System.Data;
using CattleManager.Application.Application.Common.Interfaces.Entities.Cattles;
using CattleManager.Application.Application.Validation.CustomValidations;
using FluentValidation;

namespace CattleManager.Application.Application.Validation.Cattle;

public class EditCattleValidator : AbstractValidator<EditCattleRequest>
{
    public EditCattleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SexId).NotNull().SexIdShouldBe0Or1();
        RuleFor(x => x.Breeds).NotEmpty().HasUniqueBreeds(x => x.BreedId);
        RuleFor(x => x.PurchaseDate).DateShouldNotBeInTheFuture().When(x => x.PurchaseDate is not null);
        RuleFor(x => x.DateOfBirth).DateShouldNotBeInTheFuture().When(x => x.DateOfBirth is not null);
        RuleFor(x => x.YearOfBirth).YearShouldNotBeGreaterThanCurrent()
            .Must((x, _) => DateValidator.YearInDateOfBirthShouldBeEqualToBirthYear(x.DateOfBirth, x.YearOfBirth))
            .When(x => x.DateOfBirth is not null)
            .WithMessage("Ano na data de nascimento e ano de nascimento não coincidem.");
        RuleFor(x => x.Image).MaximumLength(1000);
        RuleFor(x => x.DateOfDeath).DateShouldNotBeInTheFuture().When(x => x.DateOfDeath is not null);
        RuleFor(x => x.CauseOfDeath).MaximumLength(255);
        RuleFor(x => x.DateOfSale).DateShouldNotBeInTheFuture().When(x => x.DateOfSale is not null);
        RuleFor(x => x.PriceInCentsInReais).GreaterThanOrEqualTo(0);
    }
}