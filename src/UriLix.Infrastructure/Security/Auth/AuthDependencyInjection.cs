using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Infrastructure.Security.Options;
using UriLix.Infrastructure.Security.Providers;
using UriLix.Persistence;

namespace UriLix.Infrastructure.Security.Auth;

/// <summary>
/// Provides extension methods for configuring authentication and identity services 
/// in the dependency injection container.
/// </summary>
public static class AuthDependencyInjection
{
    /// <summary>
    /// Configures the authentication provider with Identity, JWT, and external authentication (GitHub).
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The application configuration containing authentication settings.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddIdentityAuthProvider(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddJwtBearer()
            .AddGitHub(configuration)
            .AddIdentityCookies();

        services
            .Configure<JwtOptions>(configuration.GetSection("Jwt"))
            .ConfigureOptions<JwtBearerConfigureOptions>();

        // Add Identity
        services.AddIdentityCore<ApplicationUser>()
            .AddSignInManager()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Add JWT provider
        services.AddScoped<IJWTProvider, JWTProvider>();

        return services;
    }
    /// <summary>
    /// Configures GitHub OAuth authentication using the provided configuration.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add services to.</param>
    /// <param name="configuration">The application configuration containing GitHub OAuth settings.</param>
    /// <returns>The same authentication builder so that multiple calls can be chained.</returns>
    internal static AuthenticationBuilder AddGitHub(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services.Configure<GitHubAuthOptions>(configuration.GetSection("Authentication:GitHub"));
        builder.Services.ConfigureOptions<GitHubAuthConfigureOptions>();

        // Options are configured in the GitHubAuthConfigureOptions class
        builder.AddOAuth(GitHubAuthenticationDefaults.AuthenticationScheme, delegate { });
        return builder;
    }
}
