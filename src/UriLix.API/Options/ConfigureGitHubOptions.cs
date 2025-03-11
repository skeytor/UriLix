using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System.Security.Claims;

namespace UriLix.API.Options;

internal class ConfigureGitHubOptions(IOptions<GitHubOptions> options) : IConfigureNamedOptions<OAuthOptions>
{
    private readonly GitHubOptions _gitHubOptions = options.Value;
    public void Configure(string? name, OAuthOptions options)
    {
        throw new NotImplementedException();
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

    }
}
