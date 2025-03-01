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
    public async Task<Results<Created<Guid>, BadRequest>> CreateAsync([FromBody] CreateUserRequest request)
    {
        var result = await userService.RegisterAsync(request);
        return result.IsSuccess
            ? TypedResults.Created(nameof(CreateAsync), result.Value)
            : TypedResults.BadRequest();
    }
}
