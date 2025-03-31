using Microsoft.AspNetCore.Mvc;
using UriLix.Shared.Enums;
using UriLix.Shared.Results;

namespace UriLix.API.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is successful");
        }
        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(CreateProblemDetails(result.Error)),
            ErrorType.Validation => BadRequest(CreateProblemDetails(result.Error)),
            ErrorType.Conflict => Conflict(CreateProblemDetails(result.Error)),
            ErrorType.Failure => BadRequest(CreateProblemDetails(result.Error)),
            _ => StatusCode(500, result.Error)
        };
    }
    private static ProblemDetails CreateProblemDetails(
        Error error,
        int? status = null,
        Error[]? errors = null) =>
    errors is not null
    ? new()
    {
        Title = "One or more validations occurred.",
        Type = error.Type.ToString(),
        Status = status,
        Detail = error.Description,
        Extensions = { { nameof(errors), errors } },
    }
    : new()
    {
        Title = "One or more validations occurred.",
        Type = error.Type.ToString(),
        Status = status,
        Detail = error.Description,
        Extensions = { { nameof(errors), new[] { error } } },
    };
}
