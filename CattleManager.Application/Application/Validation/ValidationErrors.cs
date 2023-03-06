using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CattleManager.Application.Application.Validation;

public static class ValidationErrors
{
    public static ModelStateDictionary GenerateModelStateDictionary(ValidationResult validationResult)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (ValidationFailure failure in validationResult.Errors)
        {
            modelStateDictionary.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }
        return modelStateDictionary;
    }
}