using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Services.Authentication;
using UriLix.Application.Services.ClickStatistics;
using UriLix.Application.Services.UrlShortening;
using UriLix.Application.Services.UrlShortening.Shortening;
using UriLix.Application.Services.Users;

namespace UriLix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlShorteningService, UrlShorteningService>();
        services.AddScoped<IShortenUrlService, ShortenUrlService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClickTrackingService, ClickTrackingService>();
        return services;
    }
    public static IServiceCollection AddCache(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddHybridCache();
        return services;
    }
}
