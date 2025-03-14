using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiBaseController
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
        if (!result.Succeeded)
        {
            return BadRequest();
        }
        var claims = result.Principal.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
