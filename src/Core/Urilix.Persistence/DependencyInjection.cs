using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;
using UriLix.Persistence.Repositories;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Persistence;

/// <summary>
/// Dependency injection extension methods for the persistence layer.
/// </summary>
public static class DependencyInjection
{
    private const string _sectionName = "Database";

    /// <summary>
    /// Registers all repository implementations and related database services with the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext>(options => options.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(options => options.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClickTrackingRepository, ClickTrackingRepository>();
        return services;
    }
    /// <summary>
    /// Configures the database provider for the application using SQL Server.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The application configuration containing the database connection string.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddDatabaseProvider(
        this IServiceCollection services, 
        IConfiguration configuration)
        => services.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString(_sectionName));
}
