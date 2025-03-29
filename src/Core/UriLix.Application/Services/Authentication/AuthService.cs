using Microsoft.AspNetCore.Identity;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Authentication;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJWTProvider tokenProvider) : IAuthService
{
    public async Task<Result<string>> SignIn(LoginRequest request)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<string>(Error.Validation(
                "User.InvalidCredentials", 
                "Invalid credentials"));
        }
        if (await userManager.IsLockedOutAsync(user))
        {
            return Result.Failure<string>(Error.Validation("User.LockedOut", "User is locked out"));
        }
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);
            if (await userManager.IsLockedOutAsync(user))
            {
                return Result.Failure<string>(Error.Validation("User.LockedOut", "User is locked out"));
            }
            return Result.Failure<string>(Error.Validation(
                "User.InvalidCredentials",
                "Invalid credentials"));
        }
        await userManager.ResetAccessFailedCountAsync(user);
        string token = tokenProvider.GenerateToken(user);
        return token;
    }
}
