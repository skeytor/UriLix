using UriLix.Application.DOTs;
using UriLix.Application.Helpers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening;

public class UrlShorteningService(
    IShortenedUrlRepository shortUrlRepository,
    IUnitOfWork unitOfWork) : IUrlShorteningService
{
    public async Task<Result<string>> ShortenUrlAsync(CreateShortenedUrlRequest request)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
        {
            return Result.Failure<string>(Error.Failure("Url.Invalid", "Invalid URL Format"));
        }
        string shortCode = ShortUrlHelper.GenerateShortUrl();
        ShortenedUrl shortenedUrl = new()
        {
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            UserId = request.UserId
        };
        await shortUrlRepository.InsertAsync(shortenedUrl);
        await unitOfWork.SaveChangesAsync();
        return shortCode;
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
