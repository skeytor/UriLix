using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using UriLix.Infrastructure.Security.Auth;

namespace UriLix.Infrastructure.Security.Options;

internal sealed class GitHubAuthConfigureOptions(IOptions<GitHubAuthOptions> gitHubOptions) 
    : IConfigureNamedOptions<OAuthOptions>
{
    private readonly GitHubAuthOptions gitHubOptions = gitHubOptions.Value;
    public void Configure(string? name, OAuthOptions options)
    {
        if (string.Equals(name, GitHubAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            Configure(options);
        }
    }
    public void Configure(OAuthOptions options)
    {
        
        options.ClientId = gitHubOptions.ClientId;
        options.ClientSecret = gitHubOptions.ClientSecret;

        options.AuthorizationEndpoint = gitHubOptions.AuthorizationEndpoint;
        options.TokenEndpoint = gitHubOptions.TokenEndpoint;
        options.UserInformationEndpoint = gitHubOptions.UserInformationEndpoint;

        options.SaveTokens = true;
        options.CallbackPath = gitHubOptions.CallbackPath;

        options.ClaimActions.MapJsonKey("sub", "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");

        options.Events.OnCreatingTicket = async (ctx) =>
        {
            using HttpRequestMessage request = new(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using HttpResponseMessage result = await ctx.Backchannel.SendAsync(request);
            JsonElement user = await result.Content.ReadFromJsonAsync<JsonElement>();
            ctx.RunClaimActions(user);
        };
    }
}
