using Microsoft.EntityFrameworkCore;
using UriLix.Domain.Entities;

namespace UriLix.Persistence.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<ShortenedUrl> ShortenedUrl { get; }
    DbSet<ClickStatistic> ClickStatistics { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
