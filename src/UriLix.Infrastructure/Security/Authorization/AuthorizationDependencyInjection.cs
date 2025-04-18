using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace UriLix.Infrastructure.Security.Authorization;

/// <summary>
/// Provides extension methods for configuring authorization policies and handlers.
/// </summary>
public static class AuthorizationDependencyInjection
{
    /// <summary>
    /// Configures authorization policies and registers authorization handlers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("EditPolicy",
                policy => policy.Requirements.Add(new SameUserRequirement()));
        services.AddSingleton<IAuthorizationHandler, ShortenedUrlAuthorizationHandler>();
        return services;
    }
}
