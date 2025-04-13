using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Hybrid;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Application.Providers;
using UriLix.Application.Services.ClickStatistics;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Pagination;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening;

public class UrlShorteningService(
    IShortenedUrlRepository shortenedUrlRepository,
    IUrlShortingProvider urlShortingProvider,
    IClickTrackingService clickTrackingService,
    HybridCache hybridCache,
    IUnitOfWork unitOfWork) : IUrlShorteningService
{
    private const int MAX_ATTEMPTS = 3;

    public async Task<Result<string>> ShortenUrlAsync(
        CreateShortenUrlRequest request, ClaimsPrincipal? principal = null)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<string>(Error.Failure(
                "Url.Invalid",
                "Invalid URL Format"));
        }
        ShortenedUrl shortenedUrl = request.ToEntity();
        if (principal is not null && principal.Identity?.IsAuthenticated == true)
        {
            shortenedUrl.UserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Check if the user provided a custom alias
        if (!string.IsNullOrWhiteSpace(request.Alias))
        {
            if (await shortenedUrlRepository.ShortUrlExistsAsync(request.Alias))
            {
                return Result.Failure<string>(Error.Failure(
                    "Alias.Duplicate",
                    $"Alias with name: {request.Alias} already exists"));
            }
            shortenedUrl.ShortCode = request.Alias;
            await shortenedUrlRepository.InsertAsync(shortenedUrl);
            await unitOfWork.SaveChangesAsync();
            await hybridCache.SetAsync(request.Alias, shortenedUrl);
            return request.Alias;
        }

        // If no custom alias is provided, generate a short code
        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            string shortCode = urlShortingProvider.GenerateShortCode();
            if (await shortenedUrlRepository.ShortUrlExistsAsync(shortCode))
            {
                continue;
            }
            shortenedUrl.ShortCode = shortCode;
            await shortenedUrlRepository.InsertAsync(shortenedUrl);
            await unitOfWork.SaveChangesAsync();
            await hybridCache.SetAsync(shortenedUrl.ShortCode, shortenedUrl);
            return shortCode;
        }
        return Result.Failure<string>(
            Error.Failure(
                "ShortCode.Duplicate",
                $"Failed to generate a unique short code after {MAX_ATTEMPTS} attempts"));
    }
    public async Task<Result<string>> GetOriginalUrlAsync(string alias, HttpRequest request)
    {
        ShortenedUrl? url = await hybridCache.GetOrCreateAsync(alias, async entry =>
        {
            return await shortenedUrlRepository.FindByShortCodeAsync(alias);
        },
        tags: ["codes"]);
        if (url is null)
        {
            return Result.Failure<string>(Error.NotFound(
                "Url.NotFound",
                $"The URL with alias: {alias} was not found"));
        }
        await clickTrackingService.RecordClickAsync(url, request);
        return url.OriginalUrl;
    }
    public async Task<Result<Guid>> UpdateAsync(Guid id, UpdateShortenUrlRequest request)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByIdAsync(id);
        if (shortenedUrl is null)
        {
            return Result.Failure<Guid>(Error.NotFound(
                "Url.NotFound",
                $"The URL with id: {id} was not found"));
        }
        shortenedUrl.OriginalUrl = request.OriginalUrl;
        await unitOfWork.SaveChangesAsync();
        return shortenedUrl.Id;
    }
    public async Task<Result<PagedResult<ShortenedUrlResponse>>> GetAllPagedAsync(
        ClaimsPrincipal principal, 
        PaginationQuery paginationQuery)
    {
        string userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        IReadOnlyList<ShortenedUrlResponse> data = (await shortenedUrlRepository
            .GetAllByUserIdAsync(userId, paginationQuery))
            .ToResponse();
        int totalCount = await shortenedUrlRepository.CountByUserIdAsync(userId);
        return PagedResult<ShortenedUrlResponse>.Create(
            data, 
            paginationQuery.Page, 
            paginationQuery.PageSize, 
            totalCount);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, ClaimsPrincipal principal)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByIdAsync(id);
        if (shortenedUrl is null)
        {
            return Result.Failure<Guid>(Error.NotFound(
                "Url.NotFound",
                $"The URL with id: {id} was not found"));
        }
        shortenedUrlRepository.Delete(id, shortenedUrl);
        await unitOfWork.SaveChangesAsync();
        return id;
    }
}
