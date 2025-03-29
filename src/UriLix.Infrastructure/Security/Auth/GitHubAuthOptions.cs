using Microsoft.AspNetCore.Http;

namespace UriLix.Infrastructure.Security.Auth;

public sealed class GitHubAuthOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AuthorizationEndpoint { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public string UserInformationEndpoint { get; set; } = string.Empty;
    public PathString CallbackPath { get; set; } = string.Empty;
}
