using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Authentication;

public interface IAuthService
{
    Task<Result<string>> SingInAsync(LoginRequest request);
}
