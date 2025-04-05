using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IShortenedUrlRepository
{
    Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedUrl);
    void Update(ShortenedUrl shortenedUrl);
    void Delete(Guid id, ShortenedUrl shortenedUrl);
    Task<ShortenedUrl?> FindByIdAsync<TProperty>(
        Guid id, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    ValueTask<ShortenedUrl?> FindByIdAsync(Guid id);
    ValueTask<ShortenedUrl?> FindByShortCodeAsync(string shortCode);
    Task<List<ShortenedUrl>> GetURLsByUserId<TProperty>(
        string userId, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    Task<List<ShortenedUrl>> GetURLsByUserId(string userId);
    Task<string?> GetOriginalUrlAsync(string code);
    Task<bool> ShortUrlExistsAsync(string shortCode);
}
