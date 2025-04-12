using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

internal static class UserExtension
{
    internal static ApplicationUser ToEntity(this CreateUserRequest request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };
    internal static UserProfileResponse ToResponse(this ApplicationUser user)
        => new($"{user.FirstName} {user.LastName}", user.Email!);
}
