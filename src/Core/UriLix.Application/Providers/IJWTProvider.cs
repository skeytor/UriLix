using UriLix.Domain.Entities;

namespace UriLix.Application.Providers;

public interface IJWTProvider
{
    string GenerateToken(ApplicationUser user);
}
