using UriLix.Domain.Entities;
using UriLix.Shared.Pagination;

namespace UriLix.Domain.Repositories;

public interface IShortenedUrlRepository
{
    Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedUrl);
    ValueTask<ShortenedUrl?> FindByIdAsync(Guid id);
    Task<ShortenedUrl?> FindByShortCodeAsync(string shortCode);
    Task<IReadOnlyList<ShortenedUrl>> GetAllByUserIdAsync(string userId, PaginationQuery paginationQuery);
    Task<int> CountByUserIdAsync(string userId);
    Task<string?> GetOriginalUrlAsync(string code);
    Task<bool> ShortUrlExistsAsync(string shortCode);
    void Update(ShortenedUrl shortenedUrl);
    void Delete(Guid id, ShortenedUrl shortenedUrl);
}
