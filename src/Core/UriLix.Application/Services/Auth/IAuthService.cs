using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Auth;

public interface IAuthService
{
    public Task<Result<JwtAccessTokenResponse>> SignIn(LoginRequest request);
    public Task<Result<JwtAccessTokenResponse>> SigInWithOAuth();
}
