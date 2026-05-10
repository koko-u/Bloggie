using System.Collections.Generic;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bloggie.Web.Extensions;

public static class FluentValidationErrorExtension
{
    public static void AddModelErrorFrom(
        this ModelStateDictionary modelState,
        IEnumerable<ValidationFailure> errors,
        string? prefix = null
    )
    {
        foreach (var error in errors)
        {
            var key = string.IsNullOrWhiteSpace(prefix)
                ? error.PropertyName
                : $"{prefix}.{error.PropertyName}";

            modelState.AddModelError(key, error.ErrorMessage);
        }
    }
}
