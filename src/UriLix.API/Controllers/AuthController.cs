using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(SignInManager<ApplicationUser> signInManager) : ApiBaseController
{
    //[HttpPost("login")]
    //[ValidateAntiForgeryToken]
    //public async Task<string> SingIn([FromBody] LoginRequest request)
    //{
    //    t
    //}
}
