using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.API.Extensions;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UsersController(IUserService userService) : ApiBaseController
{
    [HttpPost("register")]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Created<string>, BadRequest>> Register(
        [FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateAsync(request);
        return result.IsSuccess
            ? TypedResults.Created(nameof(GetUserInfo), result.Value)
            : TypedResults.BadRequest();
    }

    [Authorize]
    [HttpGet("me", Name = nameof(GetUserInfo))]
    [ProducesResponseType<UserProfileResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<UserProfileResponse>, BadRequest<ValidationProblemDetails>>> GetUserInfo()
    {
        var result = await userService.GetUserAsync(HttpContext.User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }
}
