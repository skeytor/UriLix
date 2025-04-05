using Microsoft.Extensions.DependencyInjection;

namespace UriLix.Infrastructure.Security.Authorization;

public static class AuthorizationDependencyInjection
{
    public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
            .AddPolicy("User", policy => policy.RequireRole("User"));
        return services;
    }
}
