using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.Users;

public interface IUserService
{
    Task<Result<Guid>> RegisterAsync(CreateUserRequest request);
}
