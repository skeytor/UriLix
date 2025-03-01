using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

internal static class UserExtension
{
    internal static User MapToEntity(this CreateUserRequest request) 
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };
}
