using System.Security.Claims;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.Auth;

public class AuthService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<Result<string>> HandleOAuthAsync(IEnumerable<Claim> claims)
    {
        throw new NotImplementedException();
    }
}
