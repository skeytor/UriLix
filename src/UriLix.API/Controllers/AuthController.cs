using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Authentication;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ApiBaseController
{
    [HttpPost("login")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequest>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<AccessTokenResponse>, BadRequest, UnauthorizedHttpResult>> SingIn(
        [FromBody] LoginRequest request)
    {
        var result = await authService.SignIn(request);
        AccessTokenResponse response = new()
        {
            AccessToken = result.Value,
            ExpiresIn = 3600,
            RefreshToken = Guid.NewGuid().ToString()
        };
        return result.IsSuccess
            ? TypedResults.Ok(response)
            : TypedResults.Unauthorized();
    }
}
