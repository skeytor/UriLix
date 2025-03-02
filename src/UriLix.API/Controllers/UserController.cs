using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UserController(IUserService userService) : ApiBaseController
{
    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<CreatedAtRoute<Guid>, BadRequest>> CreateAsync([FromBody] CreateUserRequest request)
    {
        var result = await userService.RegisterAsync(request);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(result.Value, nameof(GetUserProfile), new { id = result.Value })
            : TypedResults.BadRequest();
    }

    [HttpGet("{id:guid}", Name = nameof(GetUserProfile))]
    public async Task<Results<Ok<UserProfileResponse>, NotFound, BadRequest>> GetUserProfile(
        [FromRoute] Guid id)
    {
        var result = await userService.GetProfileAsync(id);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
