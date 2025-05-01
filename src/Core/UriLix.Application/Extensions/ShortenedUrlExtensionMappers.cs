using UriLix.Application.DOTs;
using UriLix.Domain.Entities;

namespace UriLix.Application.Extensions;

/// <summary>
/// Provides extension methods for mapping between different representations of shortened URLs.
/// </summary>
internal static class ShortenedUrlExtensionMappers
{
    /// <summary>
    /// Maps a <see cref="CreateShortenUrlRequest"/> to a <see cref="ShortenedUrl"/> entity.
    /// </summary>
    /// <param name="source">The request DTO</param>
    /// <returns>
    /// A new <see cref="ShortenedUrl"/> entry with the original URL and optional alias.
    /// </returns>
    internal static ShortenedUrl ToEntity(this CreateShortenUrlRequest source)
        => new()
        {
            OriginalUrl = source.OriginalUrl,
            ShortCode = source.Alias ?? string.Empty
        };
    /// <summary>
    /// Maps a <see cref="ShortenedUrl"/> entity to a <see cref="ShortenedUrlResponse"/> DTO.
    /// </summary>
    /// <param name="source">The entity to convert</param>
    /// <returns>
    /// A new <see cref="ShortenedUrlResponse"/> containing the details of the shortened URL.
    /// </returns>
    internal static ShortenedUrlResponse ToResponse(this ShortenedUrl source)
        => new(
            source.Id, 
            source.ShortCode, 
            source.OriginalUrl,
            source.ClickStatistics.Count,
            source.CreateAt, 
            source.UpdateAt);

    /// <summary>
    /// Maps a collection of <see cref="ShortenedUrl"/> entities to a read-only list of <see cref="ShortenedUrlResponse"/> DTOs.
    /// </summary>
    /// <param name="source">The collection of entities to convert.</param>
    /// <returns>An <see cref="IReadOnlyList{T}"/> of response DTOs in the same order as the input collection.</returns>
    internal static IReadOnlyList<ShortenedUrlResponse> ToResponse(this IReadOnlyList<ShortenedUrl> source)
        => [.. source.Select(ToResponse)];
}
