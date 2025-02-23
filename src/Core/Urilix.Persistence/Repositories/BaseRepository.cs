using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public abstract class BaseRepository(IAppDbContext _context)
{
    protected readonly IAppDbContext context = _context;
    protected IQueryable<TEntity> GetRelates<TEntity, TProperty>(
        params Expression<Func<TEntity, TProperty>>[] includes) where TEntity : class
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, related) => current.Include(related));
        return query;
    }
}
