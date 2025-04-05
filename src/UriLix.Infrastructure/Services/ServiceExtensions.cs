using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Providers;
using UriLix.Infrastructure.Services.Providers;

namespace UriLix.Infrastructure.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddServicesProviders(this IServiceCollection services)
    {
        services.AddScoped<IUrlShortingProvider, UrlShortingProvider>();
        return services;
    }
}
