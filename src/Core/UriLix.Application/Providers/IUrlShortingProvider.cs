namespace UriLix.Application.Providers;

/// <summary>
/// Provider for generating short codes for URLs.
/// </summary>
public interface IUrlShortingProvider
{
    /// <summary>
    /// Generates a short code for a URL.
    /// </summary>
    /// <returns></returns>
    string GenerateShortCode();
}
