using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class ShortenedUrlRepository(IAppDbContext _context) 
    : BaseRepository(_context), IShortenedUrlRepository
{
    public void DeleteAsync(Guid id, ShortenedUrl shortenedLink) 
        => context.ShortenedUrl.Remove(shortenedLink);

    public async Task<ShortenedUrl?> FindByIdAsync<TProperty>(
        Guid id, 
        params Expression<Func<ShortenedUrl, TProperty>>[] includes)
    {
        IQueryable<ShortenedUrl> query = GetRelates(includes);
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ShortenedUrl?> FindByIdAsync(Guid id) 
        => await context.ShortenedUrl.FindAsync(id);

    public async Task<List<ShortenedUrl>> GetLinksByUser<TProperty>(
        Guid userId, 
        params Expression<Func<ShortenedUrl, TProperty>>[] includes)
    {
        IQueryable<ShortenedUrl> query = GetRelates(includes);
        return await query
                     .Where(x => x.UserId == userId)
                     .AsNoTracking()
                     .ToListAsync();
    }

    public async Task<List<ShortenedUrl>> GetLinksByUser(Guid userId)
    {
        return await context
                    .ShortenedUrl
                    .Where(x =>  x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
    }

    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
        {
            return null;
        }
        return await context.ShortenedUrl
            .Where(x => x.ShortCode == shortCode)
            .Select(x => x.OriginalUrl)
            .FirstOrDefaultAsync();
    }

    public async Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedLink)
    {
        await context.ShortenedUrl.AddAsync(shortenedLink);
        return shortenedLink;
    }

    public void UpdateAsync(ShortenedUrl shortenedLink) 
        => context.ShortenedUrl.Update(shortenedLink);
}
