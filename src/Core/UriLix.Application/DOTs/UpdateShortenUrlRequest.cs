using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record UpdateShortenUrlRequest([Required, Url] string OriginalUrl);
