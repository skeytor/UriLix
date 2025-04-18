namespace UriLix.Application.DOTs;

/// <summary>
/// Represents the response containing details of a shortened URL.
/// </summary>
/// <param name="Id">The unique identifier for the shortened URL entry</param>
/// <param name="ShortCode"></param>
/// <param name="OriginalUrl">The original long URL that was shortened.</param>
/// <param name="CreatedAt">The UTC date and time when the shortened URL was created.</param>
/// <param name="UpdateAt">The UTC date and time when the shortened URL was last updated.</param>
public sealed record ShortenedUrlResponse(
    Guid Id, 
    string ShortCode,
    string OriginalUrl,
    DateTime CreatedAt,
    DateTime UpdateAt);
