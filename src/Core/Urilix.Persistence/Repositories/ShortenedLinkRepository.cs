using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class ShortenedLinkRepository(IAppDbContext _context) : BaseRepository(_context), IShortenedLinkRepository
{
    public void DeleteAsync(Guid id, ShortenedLink shortenedLink) 
        => context.ShortenedLinks.Remove(shortenedLink);

    public async Task<bool> ExistsLinkAsync(Guid linkId) 
        => await context.ShortenedLinks.AnyAsync(x => x.Id == linkId);

    public async Task<ShortenedLink?> FindByIdAsync<TProperty>(
        Guid id, 
        params Expression<Func<ShortenedLink, TProperty>>[] includes)
    {
        IQueryable<ShortenedLink> query = GetRelates(includes);
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ShortenedLink?> FindByIdAsync(Guid id) 
        => await context.ShortenedLinks.FindAsync(id);

    public async Task<List<ShortenedLink>> GetLinksByUser<TProperty>(
        Guid userId, 
        params Expression<Func<ShortenedLink, TProperty>>[] includes)
    {
        IQueryable<ShortenedLink> query = GetRelates(includes);
        return await query
                     .Where(x => x.UserId == userId)
                     .AsNoTracking()
                     .ToListAsync();
    }

    public async Task<List<ShortenedLink>> GetLinksByUser(Guid userId)
    {
        return await context
                    .ShortenedLinks
                    .Where(x =>  x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
    }

    public async Task<ShortenedLink> InsertAsync(ShortenedLink shortenedLink)
    {
        await context.ShortenedLinks.AddAsync(shortenedLink);
        return shortenedLink;
    }

    public void UpdateAsync(ShortenedLink shortenedLink) 
        => context.ShortenedLinks.Update(shortenedLink);
}
