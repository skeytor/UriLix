using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening;

public interface IUrlShorteningService
{
    public Task<Result<CreateShortenedUrlResponse>> ShortenUrlAsync(CreateShortenedUrlRequest request);
    public Task<Result<string>> GetOriginalUrlAsync(OriginalUrlQueryParam filter);   
    public Task<IReadOnlyList<GetShortenedUrlResponse>> GetAllURLsAsync(Guid userId);
}
