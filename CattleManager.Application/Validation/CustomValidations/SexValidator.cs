using FluentValidation;

namespace CattleManager.Application.Application.Validation.CustomValidations;

public static class SexValidator
{
    public static IRuleBuilderOptions<T, byte> SexIdShouldBe0Or1<T>(
        this IRuleBuilder<T, byte> ruleBuilder)
    {
        return ruleBuilder.Must(sex => sex == 0 || sex == 1).WithMessage("Código de sexo inválido, id do sexo deve ser 0 ou 1.");
    }
}