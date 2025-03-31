using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record UpdateShortenedUrlRequest([Required, Url] string OriginalUrl);
