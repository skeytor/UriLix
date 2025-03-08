using System.Linq.Expressions;
using UriLix.Application.DOTs;
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
    public async Task<Result<CreateShortenedUrlResponse>> ShortenUrlAsync(CreateShortenedUrlRequest request)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<CreateShortenedUrlResponse>(Error.Failure("Url.Invalid", "Invalid URL Format"));
        }
        ShortenedUrl shortenedUrl = new()
        {
            OriginalUrl = request.OriginalUrl,
            UserId = request.UserId,
        };
        // Check if the user provided a custom alias
        if (!string.IsNullOrWhiteSpace(request.Alias))
        {
            if (await shortenedUrlRepository.AliasExistsAsync(request.Alias))
            {
                return Result.Failure<CreateShortenedUrlResponse>(Error.Validation(
                    "Alias.Duplicate",
                    $"Alias with name: {request.Alias} already exists"));
            }
            shortenedUrl.Alias = request.Alias;
            await shortenedUrlRepository.InsertAsync(shortenedUrl);
            await unitOfWork.SaveChangesAsync();
            return new CreateShortenedUrlResponse(request.Alias, FilterType.Alias);
        }
        // If no custom alias is provided, generate a short code
        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            string shortCode = urlShortingProvider.GenerateShortCode();
            if (!await shortenedUrlRepository.ShortCodeExistsAsync(shortCode))
            {
                shortenedUrl.ShortCode = shortCode;
                await shortenedUrlRepository.InsertAsync(shortenedUrl);
                await unitOfWork.SaveChangesAsync();
                return new CreateShortenedUrlResponse(shortCode, FilterType.ShortCode);
            }
        }
        return Result.Failure<CreateShortenedUrlResponse>(
            Error.Validation(
                "ShortCode.Duplicate",
                $"Failed to generate a unique short code after {MAX_ATTEMPTS} attempts"));
    }

    public Task<IReadOnlyList<GetShortenedUrlResponse>> GetAllURLsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> GetOriginalUrlAsync(GetOriginalUrlQueryParam queryParams)
    {
        Expression<Func<ShortenedUrl, bool>> predicate = queryParams.Type switch
        {
            FilterType.Alias => x => x.Alias == queryParams.Code,
            FilterType.ShortCode => x => x.ShortCode == queryParams.Code,
            _ => x => false,
        };
        string? url = await shortenedUrlRepository.GetOriginalUrlByAsync(predicate);
        if (url is null)
        {
            return Result.Failure<string>(Error.NotFound(
                "Url.NotFound", 
                $"The code: {queryParams.Code} was not found"));
        }
        return url;
    }
}
