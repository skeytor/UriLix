using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiBaseController
{
    [HttpGet("git-hub/login")]
    public IActionResult LoginWithGitHub()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:5001" }, "GitHub");
    }
}
