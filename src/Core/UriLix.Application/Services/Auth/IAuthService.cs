using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Auth;

public interface IAuthService
{
    Task<Result<string>> HandleOAuthAsync(IEnumerable<Claim> claims);
}
