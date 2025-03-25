using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

internal static class UserExtension
{
    internal static ApplicationUser MapToEntity(this CreateUserRequest request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };
    internal static UserProfileResponse MapToResponse(this ApplicationUser user)
        => new($"{user.FirstName} {user.LastName}", user.Email!);
}
