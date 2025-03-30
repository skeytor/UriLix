using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Authentication;
using UriLix.API.Extensions;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ApiBaseController
{
    [HttpPost("login")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<AccessTokenResponse>, NotFound, BadRequest<ValidationProblemDetails>>> SingIn(
        [FromBody] LoginRequest request)
    {
        var result = await authService.SignIn(request);
        return result.IsSuccess
            ? TypedResults.Ok(new AccessTokenResponse
            {
                AccessToken = result.Value,
                ExpiresIn = 3600,
                RefreshToken = Guid.NewGuid().ToString()
            })
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }
}