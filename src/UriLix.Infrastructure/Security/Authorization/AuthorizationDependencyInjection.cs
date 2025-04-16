using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace UriLix.Infrastructure.Security.Authorization;

public static class AuthorizationDependencyInjection
{
    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("EditPolicy",
                policy => policy.Requirements.Add(new SameUserRequirement()));
        services.AddSingleton<IAuthorizationHandler, ShortenedUrlAuthorizationHandler>();
        return services;
    }
}
