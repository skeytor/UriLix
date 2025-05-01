using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Domain.Repositories;
using UriLix.Shared.Pagination;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.GetAll;

public class RetrieveUrlService(IShortenedUrlRepository shortenedUrlRepository) : IRetrieveUrlService
{
    public async Task<Result<PagedResult<ShortenedUrlResponse>>> ExecuteAsync(
        PaginationQuery parameters, ClaimsPrincipal user)
    {
        string userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        IReadOnlyList<ShortenedUrlResponse> data = (await shortenedUrlRepository
            .GetAllByUserIdAsync(userId, parameters))
            .ToResponse();
        int totalCount = await shortenedUrlRepository.CountByUserIdAsync(userId);
        return PagedResult<ShortenedUrlResponse>.Create(
            data,
            parameters.Page,
            parameters.PageSize,
            totalCount);
    }
}
