using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Pagination;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening;

public interface IUrlShorteningService
{
    public Task<Result<string>> ShortenUrlAsync(
        CreateShortenUrlRequest request, ClaimsPrincipal? user = null);
    public Task<Result<string>> GetOriginalUrlAsync(string Code, HttpRequest request);   
    public Task<Result<Guid>> UpdateAsync(Guid id, UpdateShortenUrlRequest request, ClaimsPrincipal user);
    public Task<Result<PagedResult<ShortenedUrlResponse>>> GetAllPagedAsync(
        ClaimsPrincipal user, 
        PaginationQuery paginationQuery);
    public Task<Result<Guid>> DeleteAsync(Guid id, ClaimsPrincipal user);
}
