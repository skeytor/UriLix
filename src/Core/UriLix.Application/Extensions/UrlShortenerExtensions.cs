using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

public static class UrlShortenerExtensions
{
    public static ShortenedUrl ToEntity(this CreateShortenedUrlRequest source) =>
        new()
        {
            OriginalUrl = source.OriginalUrl,
            ShortCode = source.Alias ?? string.Empty
        };
    public static ShortenedUrlResponse ToResponse(this ShortenedUrl source) =>
        new(source.Id, source.ShortCode);
    public static IEnumerable<ShortenedUrlResponse> ToResponse(this IEnumerable<ShortenedUrl> source) =>
        source.Select(ToResponse);
}
