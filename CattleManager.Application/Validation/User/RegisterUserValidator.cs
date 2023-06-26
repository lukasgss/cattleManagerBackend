using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using FluentValidation;

namespace CattleManager.Application.Application.Validation.User;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ConfirmPassword).NotEmpty().MaximumLength(255);
    }
}