using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.Update;

public interface IUpdateUrlService
{
    Task<Result<Guid>> ExecuteAsync(Guid id, UpdateShortenUrlRequest request, ClaimsPrincipal user);
}
