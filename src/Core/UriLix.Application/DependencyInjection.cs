using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Services.Auth;
using UriLix.Application.Services.ClickStatistics;
using UriLix.Application.Services.UrlShortening.Delete;
using UriLix.Application.Services.UrlShortening.GetAll;
using UriLix.Application.Services.UrlShortening.GetOriginalUrl;
using UriLix.Application.Services.UrlShortening.Shortening;
using UriLix.Application.Services.UrlShortening.Update;
using UriLix.Application.Services.Users;

namespace UriLix.Application;

/// <summary>
/// Dependency injection extension methods for the application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services</param>
    /// <returns>
    /// The same service collection so that multiple calls can be chained together.
    /// </returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IShortenUrlService, ShortenUrlService>();
        services.AddScoped<IUrlRedirectionService, UrlRedirectionService>();
        services.AddScoped<IUrlQueryService, UrlQueryService>();
        services.AddScoped<IUrlDeleteService, UrlDeleteService>();
        services.AddScoped<IUrlUpdateService, UrlUpdateService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClickTrackingService, ClickTrackingService>();
        return services;
    }
    /// <summary>
    /// Registers the Redis caching services with the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services</param>
    /// <returns>
    /// The same service collection so that multiple calls can be chained together.
    /// </returns>
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
