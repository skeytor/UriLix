using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Providers;

public interface IJWTProvider
{
    JwtAccessTokenResponse GenerateToken(ApplicationUser user);
}
