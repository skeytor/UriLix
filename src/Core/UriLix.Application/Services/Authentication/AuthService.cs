using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Authentication;

public class AuthService(
    IUserRepository userRepository,
    IPasswordProvider hashProvider,
    IJWTProvider tokenProvider) : IAuthService
{
    public async Task<Result<string>> SingInAsync(LoginRequest request)
    {
        var user = await userRepository.FindByAsync(x => x.Email == request.Email);
        if (user is null)
        {
            return Result.Failure<string>(Error.Validation("User.Unauthorized", "Invalid credentials"));
        }
        if (!hashProvider.VerifyPassword(request.Password, user.Password))
        {
            return Result.Failure<string>(Error.Validation("User.Unauthorized", "Invalid credentials"));
        }
        string token = tokenProvider.GenerateToken(user);
        return token;
    }
}
