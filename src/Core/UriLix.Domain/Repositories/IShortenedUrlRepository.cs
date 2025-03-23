using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IShortenedUrlRepository
{
    Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedUrl);
    void UpdateAsync(ShortenedUrl shortenedUrl);
    void DeleteAsync(Guid id, ShortenedUrl shortenedUrl);
    Task<ShortenedUrl?> FindByIdAsync<TProperty>(Guid id, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    Task<ShortenedUrl?> FindByIdAsync(Guid id);
    Task<List<ShortenedUrl>> GetLinksByUser<TProperty>(
        Guid userId, params Expression<Func<ShortenedUrl, TProperty>>[] includes);
    Task<List<ShortenedUrl>> GetUrisByUser(Guid userId);
    Task<string?> GetOriginalUrlByAsync(Expression<Func<ShortenedUrl, bool>> predicate);
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task<bool> AliasExistsAsync(string alias);
}
