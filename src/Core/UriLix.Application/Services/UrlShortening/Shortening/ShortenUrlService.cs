using Microsoft.Extensions.Caching.Hybrid;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening.Shortening;

public class ShortenUrlService(
    IShortenedUrlRepository shortenedUrlRepository,
    IUrlShortingProvider urlShortingProvider,
    HybridCache hybridCache,
    IUnitOfWork unitOfWork) : IShortenUrlService
{
    private const int MAX_ATTEMPTS = 3;
    public async Task<Result<string>> ExecuteAsync(CreateShortenUrlRequest request, ClaimsPrincipal? user = null)
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
            await SaveAndCacheUrlAsync(shortenedUrl);
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
            await SaveAndCacheUrlAsync(shortenedUrl);
            return shortCode;
        }
        return Result.Failure<string>(
            Error.Failure(
                "ShortCode.Duplicate",
                $"Failed to generate a unique short code after {MAX_ATTEMPTS} attempts"));
    }
    private async Task SaveAndCacheUrlAsync(ShortenedUrl shortenedUrl)
    {
        await shortenedUrlRepository.InsertAsync(shortenedUrl);
        await unitOfWork.SaveChangesAsync();
        await hybridCache.SetAsync(shortenedUrl.ShortCode, shortenedUrl);
    }
}