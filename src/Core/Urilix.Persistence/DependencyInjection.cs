using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;
using UriLix.Persistence.Repositories;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Persistence;

public static class DependencyInjection
{
    private const string _sectionName = "Database";

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAppDbContext>(options => options.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork>(options => options.GetRequiredService<AppDbContext>());
        services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
        return services;
    }
    public static IServiceCollection AddDatabaseProvider(
        this IServiceCollection services, 
        IConfiguration configuration)
        => services.AddSqlServer<AppDbContext>(configuration.GetConnectionString(_sectionName));
}
