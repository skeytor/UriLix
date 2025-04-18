using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Providers;

/// <summary>
/// Provider for generating JWT tokens
/// </summary>
public interface IJWTProvider
{
    /// <summary>
    /// Generates a JWT token for the given user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    JwtAccessTokenResponse GenerateToken(ApplicationUser user);
}
