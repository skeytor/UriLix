﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Infrastructure.Security.Auth;
using UriLix.Infrastructure.Security.Auth.Providers;
using UriLix.Infrastructure.Security.Options;
using UriLix.Persistence;

namespace UriLix.Infrastructure.Security.Extensions;

public static class AuthExtensions
{
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
            .AddJwtBearer()
            .AddGitHub(configuration)
            .AddIdentityCookies();

        services
            .Configure<JwtOptions>(configuration.GetSection("Jwt"))
            .ConfigureOptions<JwtBearerParametersConfigureOptions>();

        // Add Identity
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        // Add JWT provider
        services.AddScoped<IJWTProvider, JWTProvider>();

        return services;
    }
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
