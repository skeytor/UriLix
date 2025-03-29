using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Providers;
using UriLix.Application.Services.Authentication;
using UriLix.Application.Services.UrlShortening;
using UriLix.Application.Services.Users;

namespace UriLix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlShorteningService, UrlShorteningService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUrlShortingProvider, UrlShortingProvider>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
