using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

/// <summary>
/// Provides extension methods for mapping between different representations of users.
/// </summary>
internal static class UserExtensionMappers
{
    /// <summary>
    /// Maps a <see cref="CreateUserRequest"/> to a <see cref="ApplicationUser"/> entity.
    /// </summary>
    /// <param name="request">The request DTO to convert</param>
    /// <returns>
    /// A new <see cref="ApplicationUser"/> entry with the first name, last name, email, and username.
    /// </returns>
    internal static ApplicationUser ToEntity(this CreateUserRequest request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };
    /// <summary>
    /// Maps a <see cref="CreateUserRequest"/> to a <see cref="UserProfileResponse"/> DTO.
    /// </summary>
    /// <param name="user">The entity to convert</param>
    /// <returns>
    /// A new <see cref="UserProfileResponse"/> containing the user's full name and email.
    /// </returns>
    internal static UserProfileResponse ToResponse(this ApplicationUser user)
        => new($"{user.FirstName} {user.LastName}", user.Email!);
}
