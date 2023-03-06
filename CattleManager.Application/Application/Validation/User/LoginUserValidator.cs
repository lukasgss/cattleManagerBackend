using CattleManager.Application.Application.Common.Interfaces.Entities.Users;
using FluentValidation;

namespace CattleManager.Application.Application.Validation.User;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
    }
}