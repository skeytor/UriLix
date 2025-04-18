using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using UriLix.Infrastructure.Security.Auth;
using UriLix.Infrastructure.Security.Helpers;

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

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");

        options.Events.OnCreatingTicket = async (ctx) =>
        {
            JsonElement userData = await GitHubHelpers.GetUserInfo(ctx, ctx.Options.UserInformationEndpoint);
            ctx.RunClaimActions(userData);
            JsonElement emailData = await GitHubHelpers.GetUserInfo(ctx, $"{ctx.Options.UserInformationEndpoint}/emails");
            string email = emailData.EnumerateArray()
                .FirstOrDefault(e => e.GetProperty("primary").GetBoolean())
                .GetProperty("email").GetString() ?? string.Empty;
            ctx?.Identity?.AddClaim(new Claim(ClaimTypes.Email, email));
        };
    }
}
