namespace UriLix.Application.DOTs;

public sealed record ShortenedUrlResponse(
    Guid Id, 
    string ShortCode,
    string OriginalUrl,
    DateTime CreatedAt,
    DateTime UpdateAt);
