using System.Text;
using UriLix.Application.DOTs;
using UriLix.Application.Helpers;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening;

public class UrlShorteningService(
    IShortenedUrlRepository shortUrlRepository,
    IUrlShortingProvider urlShortingProvider,
    IUnitOfWork unitOfWork) : IUrlShorteningService
{
    private const int MAX_ATTEMPTS = 3;
    public async Task<Result<string>> ShortenUrlAsync(CreateShortenedUrlRequest request)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<string>(Error.Failure("Url.Invalid", "Invalid URL Format"));
        }
        ShortenedUrl shortenedUrl = new()
        {
            OriginalUrl = request.OriginalUrl,
            UserId = request.UserId,
        };
        // Check if the user provided a custom alias
        if (!string.IsNullOrWhiteSpace(request.Alias))
        {
            if (await shortUrlRepository.AliasExistsAsync(request.Alias))
            {
                return Result.Failure<string>(Error.Validation(
                    "Alias.Duplicate", 
                    $"Alias with name: {request.Alias} already exists"));
            }
            shortenedUrl.Alias = request.Alias;
            await shortUrlRepository.InsertAsync(shortenedUrl);
            await unitOfWork.SaveChangesAsync();
            return request.Alias;
        }
        // If no custom alias is provided, generate a short code
        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            string shortCode = urlShortingProvider.GenerateShortCode();
            if (!await shortUrlRepository.ShortCodeExistsAsync(shortCode))
            {
                shortenedUrl.ShortCode = shortCode;
                await shortUrlRepository.InsertAsync(shortenedUrl);
                await unitOfWork.SaveChangesAsync();
                return shortCode;
            }
        }
        return Result.Failure<string>(
            Error.Validation(
                "ShortCode.Duplicate", 
                "Failed to generate a unique short code"));
    }

    public Task<IReadOnlyList<GetShortenedUrlResponse>> GetAllURLsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> GetOriginalUrlAsync(string shortCode)
    {
        throw new NotImplementedException();
    }
}
