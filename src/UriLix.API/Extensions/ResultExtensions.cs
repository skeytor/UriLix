using Microsoft.AspNetCore.Mvc;
using UriLix.Shared.Enums;
using UriLix.Shared.Results;

namespace UriLix.API.Extensions;

internal static class ResultExtensions
{
    internal static ValidationProblemDetails ToValidationProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is successful");
        }
        return result.Error.Type switch
        {
            ErrorType.NotFound => CreateProblemDetails(result.Error, 404),
            ErrorType.Validation => CreateProblemDetails(result.Error, 400),
            ErrorType.Conflict => CreateProblemDetails(result.Error, 409),
            ErrorType.Failure => CreateProblemDetails(result.Error, 400),
            _ => CreateProblemDetails(result.Error, 500)
        };
    }
    private static ValidationProblemDetails CreateProblemDetails(
        Error error,
        int? status = null,
        Error[]? errors = null) 
            => errors is not null
            ? new ValidationProblemDetails
            {
                Title = "One or more validations occurred.",
                Type = error.Type.ToString(),
                Status = status,
                Detail = error.Description,
                Extensions = { { nameof(errors), errors } }
            }
            : new ValidationProblemDetails
            {
                Title = "One or more validations occurred.",
                Type = error.Type.ToString(),
                Status = status,
                Extensions = { { nameof(errors), new[] { error } } }
            };
}
