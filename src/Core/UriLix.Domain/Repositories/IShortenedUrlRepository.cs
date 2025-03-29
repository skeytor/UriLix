using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IShortenedUrlRepository
{
    Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedUrl);
    void UpdateAsync(ShortenedUrl shortenedUrl);
    void DeleteAsync(Guid id, ShortenedUrl shortenedUrl);
    Task<ShortenedUrl?> FindByIdAsync<TProperty>(
        Guid id, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    Task<ShortenedUrl?> FindByIdAsync(Guid id);
    Task<ShortenedUrl?> FindByShortCodeAsync(string shortCode);
    Task<List<ShortenedUrl>> GetURLsByUserId<TProperty>(
        string userId, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    Task<List<ShortenedUrl>> GetURLsByUserId(string userId);
    Task<string?> GetOriginalUrlAsync(string code);
    Task<bool> ShortUrlExistsAsync(string shortCode);
}
