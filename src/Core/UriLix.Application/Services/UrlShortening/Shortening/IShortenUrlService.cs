using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.Shortening;

public interface IShortenUrlService
{
    Task<Result<string>> ExecuteAsync(CreateShortenUrlRequest request, ClaimsPrincipal? user = null);
}
