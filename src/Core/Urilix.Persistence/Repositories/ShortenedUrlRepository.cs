using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class ShortenedUrlRepository(IApplicationDbContext _context) 
    : BaseRepository(_context), IShortenedUrlRepository
{
    public Task<bool> AliasExistsAsync(string alias) 
        => Context.ShortenedUrl.AnyAsync(x => x.Alias == alias);

    public void DeleteAsync(Guid id, ShortenedUrl shortenedLink) 
        => Context.ShortenedUrl.Remove(shortenedLink);

    public Task<ShortenedUrl?> FindByIdAsync<TProperty>(
        Guid id, 
        params Expression<Func<ShortenedUrl, TProperty>>[] includes)
    {
        IQueryable<ShortenedUrl> query = GetIncludeEntities(includes);
        return query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ShortenedUrl?> FindByIdAsync(Guid id) 
        => await Context.ShortenedUrl.FindAsync(id);

    public Task<List<ShortenedUrl>> GetURLsByUserId<TProperty>(
        string userId, 
        params Expression<Func<ShortenedUrl, TProperty>>[] includes)
    {
        IQueryable<ShortenedUrl> query = GetIncludeEntities(includes);
        return query
              .Where(x => x.UserId == userId)
              .AsNoTracking()
              .ToListAsync();
    }

    public Task<List<ShortenedUrl>> GetURLsByUserId(string userId) 
        => Context.ShortenedUrl
                  .Where(x => x.UserId == userId)
                  .AsNoTracking()
                  .ToListAsync();

    public Task<string?> GetOriginalUrlAsync(string shortCode) 
        => Context.ShortenedUrl
                  .Where(x => x.ShortCode == shortCode)
                  .Select(x => x.OriginalUrl)
                  .FirstOrDefaultAsync();

    public Task<string?> GetOriginalUrlByAsync(Expression<Func<ShortenedUrl, bool>> predicate) 
        => Context.ShortenedUrl
                  .Where(predicate)
                  .Select(x => x.OriginalUrl)
                  .FirstOrDefaultAsync();

    public async Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedLink)
    {
        await Context.ShortenedUrl.AddAsync(shortenedLink);
        return shortenedLink;
    }

    public Task<bool> ShortCodeExistsAsync(string shortCode) 
        => Context.ShortenedUrl.AnyAsync(x => x.ShortCode == shortCode);

    public void UpdateAsync(ShortenedUrl shortenedLink) 
        => Context.ShortenedUrl.Update(shortenedLink);
}
