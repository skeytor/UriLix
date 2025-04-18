using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Auth;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJWTProvider tokenProvider) : IAuthService
{
    public async Task<Result<JwtAccessTokenResponse>> SigInWithOAuth()
    {
        ExternalLoginInfo? info = await signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            return Result.Failure<JwtAccessTokenResponse>(Error.Failure("ExternalLoginInfo.Missing", "External login information is missing."));
        }
        ApplicationUser? user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user is not null)
        {
            return tokenProvider.GenerateToken(user);
        }
        string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
        };
        IdentityResult createdResult = await userManager.CreateAsync(user);
        if (createdResult.Succeeded)
        {
            await signInManager.UserManager.AddLoginAsync(user, info);
            return tokenProvider.GenerateToken(user);
        }
        if (createdResult.Errors.Any())
        {
            return Result.Failure<JwtAccessTokenResponse>(Error.Failure(
                "User.Create",
                createdResult.Errors.First().Description));
        }
        return Result.Failure<JwtAccessTokenResponse>(Error.Failure("User.Create", "Failed to create the user due to an unknown error."));
    }

    public async Task<Result<JwtAccessTokenResponse>> SignIn(LoginRequest request)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<JwtAccessTokenResponse>(Error.Validation(
                "User.InvalidCredentials", 
                "Invalid credentials"));
        }
        if (await userManager.IsLockedOutAsync(user))
        {
            return Result.Failure<JwtAccessTokenResponse>(Error.Validation(
                "User.LockedOut", 
                "User is locked out"));
        }
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);
            if (await userManager.IsLockedOutAsync(user))
            {
                return Result.Failure<JwtAccessTokenResponse>(Error.Validation(
                    "User.LockedOut", 
                    "User is locked out"));
            }
            return Result.Failure<JwtAccessTokenResponse>(Error.Validation(
                "User.InvalidCredentials",
                "Invalid credentials"));
        }
        await userManager.ResetAccessFailedCountAsync(user);
        return tokenProvider.GenerateToken(user);
    }
}
