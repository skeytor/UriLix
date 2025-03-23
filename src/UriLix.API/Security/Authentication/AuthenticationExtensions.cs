using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using UriLix.API.Options;
using UriLix.Domain.Entities;
using UriLix.Persistence;

namespace UriLix.API.Security.Authentication;

internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            })
            .AddBearerToken(IdentityConstants.BearerScheme)
            .AddGitHub(configuration)
            .AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        return services;
    }
    internal static AuthenticationBuilder AddGitHub(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services
            .Configure<GitHubAuthenticationOptions>(configuration.GetSection("Authentication:GitHub"));

        builder.Services.ConfigureOptions<ConfigureGitHubAuthenticationOptions>();

        // Options will be configured by ConfigureGitHubOAuthOptions
        builder.AddOAuth(GitHubAuthenticationDefaults.AuthenticationScheme, delegate { });
        return builder;
    }
}
