namespace UriLix.Application.DOTs;

/// <summary>
/// Represents a JWT authentication response containing access tokens.
/// </summary>
/// <param name="AccessToken">The JSON Web Token (JWT)</param>
/// <param name="ExpiresIn">The duration in seconds until the access token expires.</param>
/// <param name="RefreshToken">Token used to obtain a new access token when the current one expires.</param>
public record JwtAccessTokenResponse(string AccessToken, int ExpiresIn, string RefreshToken)
{
    /// <summary>
    /// The type of token returned. Always returns "Bearer" for JWT tokens.
    /// </summary>
    public string TokenType { get; init; } = "Bearer";
};
