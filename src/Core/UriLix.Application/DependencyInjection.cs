using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlShorteningService, UrlShorteningService>();
        return services;
    }
}
