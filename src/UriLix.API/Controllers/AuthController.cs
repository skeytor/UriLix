using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Authentication;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ApiBaseController
{
    [HttpPost("login")]
    public async Task<Results<Ok<string>, BadRequest>> LoginAsync(
        [FromBody]  LoginRequest request)
    {
        var result = await authService.SingInAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest();
    }
}
