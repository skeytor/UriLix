using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Users;

public interface IUserService
{
    Task<Result<string>> CreateAsync(CreateUserRequest request);
    Task<Result<UserProfileResponse>> GetUserAsync(ClaimsPrincipal principal);
}
