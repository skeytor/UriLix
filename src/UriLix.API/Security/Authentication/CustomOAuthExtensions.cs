using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace UriLix.API.Security.Authentication;

internal static class CustomOAuthExtensions
{
    internal static IServiceCollection AddAuthConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGitHub(configuration);
        return services;
    }
    internal static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        return builder.AddOAuth("GitHub", options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            options.ClientId = configuration["Authentication:GitHub:ClientId"]!;
            options.ClientSecret = configuration["Authentication:GitHub:ClientSecret"]!;

            options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            options.TokenEndpoint = "https://github.com/login/oauth/access_token";
            options.UserInformationEndpoint = "https://api.github.com/user";
            options.SaveTokens = true;
            options.CallbackPath = "/oauth/github-cb";

            options.ClaimActions.MapJsonKey("sub", "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");

            options.Events.OnCreatingTicket = async (ctx) =>
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
                using var result = await ctx.Backchannel.SendAsync(request);
                var user = await result.Content.ReadFromJsonAsync<JsonElement>();
                ctx.RunClaimActions(user);
            };
        });
    }
}
