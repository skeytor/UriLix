using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UserController(IUserService userService) : ApiBaseController
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

    [HttpGet("me", Name = nameof(GetUserInfo))]
    public async Task<Results<Ok<UserProfileResponse>, NotFound, BadRequest>> GetUserInfo()
    {
        var result = await userService.GetUserAsync(HttpContext.User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest();
    }
}
