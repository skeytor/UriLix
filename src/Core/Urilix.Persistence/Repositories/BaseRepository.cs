using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public abstract class BaseRepository(IApplicationDbContext context)
{
    protected readonly IApplicationDbContext Context = context;
    protected IQueryable<TEntity> GetIncludeEntities<TEntity, TProperty>(
        params Expression<Func<TEntity, TProperty>>[] includes) where TEntity : class
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, related) => current.Include(related));
        return query;
    }
}
