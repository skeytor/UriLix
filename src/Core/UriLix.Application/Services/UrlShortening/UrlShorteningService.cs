using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening;

public class UrlShorteningService(
    IShortenedUrlRepository shortenedUrlRepository,
    IUrlShortingProvider urlShortingProvider,
    IUnitOfWork unitOfWork) : IUrlShorteningService
{
    private const int MAX_ATTEMPTS = 3;
    public async Task<Result<string>> ShortenUrlAsync(
        CreateShortenedUrlRequest request, ClaimsPrincipal? principal = null)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<string>(Error.Failure(
                "Url.Invalid", 
                "Invalid URL Format"));
        }

        ShortenedUrl shortenedUrl = request.ToEntity();

        if (principal is not null and { Identity.IsAuthenticated: true })
        {
            shortenedUrl.UserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);;
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
            return shortCode;
        }
        return Result.Failure<string>(
            Error.Failure(
                "ShortCode.Duplicate",
                $"Failed to generate a unique short code after {MAX_ATTEMPTS} attempts"));
    }

    public async Task<Result<IReadOnlyList<ShortenedUrlResponse>>> GetAllURLsAsync(ClaimsPrincipal principal)
    {
        string? userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure<IReadOnlyList<ShortenedUrlResponse>>(Error.Validation("", ""));
        }
        return (await shortenedUrlRepository.GetURLsByUserId(userId))
            .ToResponse()
            .ToList()
            .AsReadOnly();

    }
    public async Task<Result<string>> GetOriginalUrlAsync(string alias)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByShortCodeAsync(alias);
        if (shortenedUrl is null)
        {
            return Result.Failure<string>(Error.NotFound(
                "Url.NotFound", 
                $"The code: {alias} was not found"));
        }
        return shortenedUrl.OriginalUrl;
    }
}
