using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using UriLix.API.Security.Authentication;

namespace UriLix.API.Options;

internal class ConfigureGitHubAuthenticationOptions(IOptions<GitHubAuthenticationOptions> options) 
    : IConfigureNamedOptions<OAuthOptions>
{
    private readonly GitHubAuthenticationOptions _gitHubOptions = options.Value;
    public void Configure(string? name, OAuthOptions options)
    {
        if (string.Equals(name, GitHubAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            Configure(options);
        }
    }

    public void Configure(OAuthOptions options)
    {
        options.ClientId = _gitHubOptions.ClientId;
        options.ClientSecret = _gitHubOptions.ClientSecret;

        options.AuthorizationEndpoint = _gitHubOptions.AuthorizationEndpoint;
        options.TokenEndpoint = _gitHubOptions.TokenEndpoint;
        options.UserInformationEndpoint = _gitHubOptions.UserInformationEndpoint;
        
        options.SaveTokens = true;
        options.CallbackPath = _gitHubOptions.CallbackPath;
        
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
    }
}
