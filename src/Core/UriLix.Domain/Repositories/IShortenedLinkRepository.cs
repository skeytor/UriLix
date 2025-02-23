using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IShortenedLinkRepository
{
    Task<ShortenedLink> InsertAsync(ShortenedLink shortenedLink);
    void UpdateAsync(ShortenedLink shortenedLink);
    void DeleteAsync(Guid id, ShortenedLink shortenedLink);
    Task<ShortenedLink?> FindByIdAsync<TProperty>(Guid id, params Expression<Func<ShortenedLink, TProperty>>[] includes);
    Task<ShortenedLink?> FindByIdAsync(Guid id);
    Task<List<ShortenedLink>> GetLinksByUser<TProperty>(
        Guid userId, params Expression<Func<ShortenedLink, TProperty>>[] includes);
    Task<List<ShortenedLink>> GetLinksByUser(Guid userId);
    Task<bool> ExistsLinkAsync(Guid linkId);
}
