using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UriLix.API.Options;

namespace UriLix.API.Security.Authentication;

internal static class CustomOAuthExtensions
{
    internal static IServiceCollection AddAuthenticationProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGitHub(configuration);
        return services;
    }
    internal static AuthenticationBuilder AddGitHub(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services.Configure<GitHubAuthenticationOptions>(configuration.GetSection("Authentication:GitHub"));

        builder.Services.ConfigureOptions<ConfigureGitHubAuthenticationOptions>();

        // Options will be configured by ConfigureGitHubOAuthOptions
        builder.AddOAuth(GitHubAuthenticationDefaults.AuthenticationScheme, delegate { });
        return builder;
    }
}
