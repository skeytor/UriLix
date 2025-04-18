using System.Text.Json.Serialization;

namespace UriLix.Infrastructure.Security.Auth;

/// <summary>
/// Represents the available external OAuth2.0 providers supported by the application.
/// </summary>
/// <remarks>
/// This enum is decorated with <see cref="JsonConverterAttribute"/> to ensure proper
/// JSON serialization/deserialization of enum values as strings.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OAuthProviders
{
    GitHub = 1,
}
