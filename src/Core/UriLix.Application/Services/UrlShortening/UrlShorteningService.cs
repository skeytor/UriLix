using Microsoft.AspNetCore.Authorization;
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
    IAuthorizationService authorizationService,
    HybridCache hybridCache,
    IUnitOfWork unitOfWork) : IUrlShorteningService
{
    private const int MAX_ATTEMPTS = 3;

    public async Task<Result<string>> ShortenUrlAsync(
        CreateShortenUrlRequest request, ClaimsPrincipal? user = null)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<string>(Error.Failure(
                "Url.Invalid",  
                "Invalid URL Format"));
        }
        ShortenedUrl shortenedUrl = request.ToEntity();
        if (user is not null && user.Identity?.IsAuthenticated == true)
        {
            shortenedUrl.UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
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
    public async Task<Result<string>> GetOriginalUrlAsync(string alias, HttpRequest httpRequestInfo)
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
        _ = Task.Run(async () => await clickTrackingService.RecordClickAsync(url, httpRequestInfo));
        return url.OriginalUrl;
    }
    public async Task<Result<Guid>> UpdateAsync(Guid id, UpdateShortenUrlRequest request, ClaimsPrincipal user)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByIdAsync(id);
        if (shortenedUrl is null)
        {
            return Result.Failure<Guid>(Error.NotFound(
                "Url.NotFound",
                $"The URL with id: {id} was not found"));
        }
        AuthorizationResult authResult = await authorizationService.AuthorizeAsync(user, shortenedUrl, "EditPolicy");
        if (!authResult.Succeeded)
        {
            return Result.Failure<Guid>(Error.Failure(
                "Url.Forbidden",
                "You are not authorized to edit this URL"));
        }

        shortenedUrl.OriginalUrl = request.OriginalUrl;
        await unitOfWork.SaveChangesAsync();
        return shortenedUrl.Id;
    }
    public async Task<Result<PagedResult<ShortenedUrlResponse>>> GetAllPagedAsync(
        ClaimsPrincipal user, 
        PaginationQuery paginationQuery)
    {
        string userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
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

    public async Task<Result<Guid>> DeleteAsync(Guid id, ClaimsPrincipal user)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByIdAsync(id);
        if (shortenedUrl is null)
        {
            return Result.Failure<Guid>(Error.NotFound(
                "Url.NotFound",
                $"The URL with id: {id} was not found"));
        }
        AuthorizationResult authResult = await authorizationService.AuthorizeAsync(user, shortenedUrl, "EditPolicy");
        if (!authResult.Succeeded)
        {
            return Result.Failure<Guid>(Error.Failure(
                "Url.Forbidden",
                "You are not authorized to delete this URL"));
        }
        shortenedUrlRepository.Delete(id, shortenedUrl);
        await unitOfWork.SaveChangesAsync();
        return id;
    }
}
