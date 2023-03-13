using FluentValidation;

namespace CattleManager.Application.Application.Validation.CustomValidations;

public static class BreedValidator
{
    public static IRuleBuilder<T, IEnumerable<TSource>> HasUniqueBreeds<T, TSource, TResult>(
        this IRuleBuilder<T, IEnumerable<TSource>> ruleBuilder,
        Func<TSource, TResult> selector)
    {
        if (selector == null)
            throw new ArgumentNullException(nameof(selector), "Cannot pass a null selector.");

        ruleBuilder
            .Must(x =>
            {
                var array = x.Select(selector).ToArray();
                return array.Length == array.Distinct().Count();
            })
            .WithMessage("Foram inseridas raças repetidas.");

        return ruleBuilder;
    }
}