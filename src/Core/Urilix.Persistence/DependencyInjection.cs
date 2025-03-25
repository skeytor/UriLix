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
        services.AddScoped<IApplicationDbContext>(options => options.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(options => options.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
    public static IServiceCollection AddDatabaseProvider(
        this IServiceCollection services, 
        IConfiguration configuration)
        => services.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString(_sectionName));
}
