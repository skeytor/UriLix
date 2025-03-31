using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Authentication;

public interface IAuthService
{
    public Task<Result<JwtAccessTokenResponse>> SignIn(LoginRequest request);
}
