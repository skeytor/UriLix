using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Auth;
using UriLix.API.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using UriLix.Domain.Entities;
using UriLix.Infrastructure.Security.Auth;
using System.ComponentModel.DataAnnotations;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(
    IAuthService authService,
    SignInManager<ApplicationUser> signInManager) : ApiBaseController
{
    [HttpPost("login")]
    [ProducesResponseType<JwtAccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Ok<JwtAccessTokenResponse>, NotFound, BadRequest<ValidationProblemDetails>>> SingIn(
        [FromBody] LoginRequest request)
    {
        var result = await authService.SignIn(request);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }

    [HttpGet("external-login")]
    public IActionResult ExternalLogin(
        [FromQuery, Required] LoginProviders provider, 
        [FromQuery] string? returnUrl = null)
    {
        string? redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { ReturnUrl = returnUrl });
        AuthenticationProperties properties = signInManager
            .ConfigureExternalAuthenticationProperties(provider.ToString(), redirectUrl);
        return Challenge(properties, provider.ToString());
    }

    [HttpGet("callback")]
    public async Task<IActionResult> ExternalLoginCallback(
        [FromQuery] string? returnUrl = null, 
        [FromQuery] string? remoteError = null)
    {
        if (remoteError is not null)
        {
            return BadRequest();
        }
        AuthenticateResult authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!authResult.Succeeded)
        {
            return BadRequest();
        }
        var result = await authService.SigInWithOAuth();
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }
}