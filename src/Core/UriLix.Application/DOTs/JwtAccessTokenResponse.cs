namespace UriLix.Application.DOTs;

public record JwtAccessTokenResponse(string AccessToken, int ExpiresIn, string RefreshToken)
{
    public string TokenType { get; init; } = "Bearer";
};
