using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Domain.Entities;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Users;

public class UserService(
    UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<Result<UserProfileResponse>> GetUserAsync(ClaimsPrincipal principal)
    {
        ApplicationUser? user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Result.Failure<UserProfileResponse>(Error.NotFound(
                "User.NotFound",
                $"User not found"));
        }
        return user.ToResponse();
    }

    public async Task<Result<string>> CreateAsync(CreateUserRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Result.Failure<string>(Error.Validation(
                "User.AlreadyExists",
                "User already exists"));
        }

        ApplicationUser user = request.ToEntity();
        
        IdentityResult result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result.Failure<string>(Error.Validation(
                "User.CreateFailed", 
                result.Errors.First().Description));
        }
        return user.Id;
    }
}
