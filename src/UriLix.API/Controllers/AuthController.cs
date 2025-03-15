using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.Services.Auth;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ApiBaseController
{
    [HttpGet("git-hub")]
    public IActionResult LoginWithGitHub()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:5001/api/Auth/callback" }, "GitHub");
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        AuthenticateResult result = await HttpContext.AuthenticateAsync();
        var token = await HttpContext.GetTokenAsync("access_token");
        if (!result.Succeeded)
        {
            return BadRequest();
        }
        //var s = await authService.HandleOAuthAsync(result.Principal.Claims);
        return Ok("");
    }
}
