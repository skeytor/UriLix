﻿using UriLix.Application.DOTs;
using UriLix.Domain.Entities;
using UriLix.Shared.Pagination;

namespace UriLix.Application.Extensions;

public static class ShortenedUrlExtensions
{
    public static ShortenedUrl ToEntity(this CreateShortenUrlRequest source)
        => new()
        {
            OriginalUrl = source.OriginalUrl,
            ShortCode = source.Alias ?? string.Empty
        };
    public static ShortenedUrlResponse ToResponse(this ShortenedUrl source)
        => new(source.Id, source.ShortCode, source.OriginalUrl, source.CreateAt, source.UpdateAt);
    public static IReadOnlyList<ShortenedUrlResponse> ToResponse(this IReadOnlyList<ShortenedUrl> source)
        => [.. source.Select(ToResponse)];

}
