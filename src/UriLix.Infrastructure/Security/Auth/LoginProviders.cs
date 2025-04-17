using System.Text.Json.Serialization;

namespace UriLix.Infrastructure.Security.Auth;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LoginProviders
{
    GitHub = 1,
}
