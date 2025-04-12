using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

internal static class ShortenedUrlExtensions
{
    internal static ShortenedUrl ToEntity(this CreateShortenUrlRequest source)
        => new()
        {
            OriginalUrl = source.OriginalUrl,
            ShortCode = source.Alias ?? string.Empty
        };
    internal static ShortenedUrlResponse ToResponse(this ShortenedUrl source)
        => new(source.Id, source.ShortCode, source.OriginalUrl, source.CreateAt, source.UpdateAt);
    internal static IReadOnlyList<ShortenedUrlResponse> ToResponse(this IReadOnlyList<ShortenedUrl> source)
        => [.. source.Select(ToResponse)];

}
