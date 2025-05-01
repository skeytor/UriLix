using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Pagination;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.GetAll;

public interface IRetrieveUrlService
{
    public Task<Result<PagedResult<ShortenedUrlResponse>>> ExecuteAsync(
        PaginationQuery parameters, 
        ClaimsPrincipal user);
}
