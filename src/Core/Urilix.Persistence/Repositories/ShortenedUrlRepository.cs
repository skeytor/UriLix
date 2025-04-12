using Microsoft.EntityFrameworkCore;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;
using UriLix.Persistence.Specifications;
using UriLix.Shared.Pagination;

namespace UriLix.Persistence.Repositories;

public class ShortenedUrlRepository(IApplicationDbContext context) 
    : BaseRepository(context), IShortenedUrlRepository
{

    public void Delete(Guid id, ShortenedUrl shortenedLink) 
        => Context.ShortenedUrl.Remove(shortenedLink);

    public async ValueTask<ShortenedUrl?> FindByIdAsync(Guid id) 
        => await Context.ShortenedUrl.FindAsync(id);

    public Task<string?> GetOriginalUrlAsync(string code) 
        => Context.ShortenedUrl
                  .Where(x => x.ShortCode == code)
                  .Select(x => x.OriginalUrl)
                  .FirstOrDefaultAsync();

    public async Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedLink)
    {
        await Context.ShortenedUrl.AddAsync(shortenedLink);
        return shortenedLink;
    }

    public Task<bool> ShortUrlExistsAsync(string shortCode) 
        => Context.ShortenedUrl.AnyAsync(x => x.ShortCode == shortCode);

    public void Update(ShortenedUrl shortenedLink) 
        => Context.ShortenedUrl.Update(shortenedLink);

    public Task<ShortenedUrl?> FindByShortCodeAsync(string shortCode) 
        => Context.ShortenedUrl.FirstOrDefaultAsync(x => x.ShortCode == shortCode);

    public async Task<IReadOnlyList<ShortenedUrl>> GetAllByUserIdAsync(string userId, PaginationQuery paginationQuery) 
        => await SpecificationEvaluator
            .GetQuery(Context.ShortenedUrl.AsQueryable(), new GetAllByUserIdSpec(userId, paginationQuery))
            .AsNoTracking()
            .ToListAsync();

    public Task<int> CountByUserIdAsync(string userId) 
        => Context.ShortenedUrl
            .AsNoTracking()
            .CountAsync(x => x.UserId == userId);
}
