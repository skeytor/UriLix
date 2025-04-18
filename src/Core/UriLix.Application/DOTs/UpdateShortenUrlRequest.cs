using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

/// <summary>
/// Request to update a shorten URL.
/// </summary>
/// <param name="OriginalUrl">The new target URL</param>
public sealed record UpdateShortenUrlRequest([Required, Url] string OriginalUrl);
