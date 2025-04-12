using UriLix.Domain.Specifications;
using UriLix.Persistence.Abstractions;
using UriLix.Persistence.Specifications;

namespace UriLix.Persistence.Repositories;

public abstract class BaseRepository(IApplicationDbContext context)
{
    protected readonly IApplicationDbContext Context = context;
    protected IQueryable<TEntity> ApplySpecification<TEntity>(Specification<TEntity> specification)
        where TEntity : class 
        => SpecificationEvaluator.GetQuery(Context.Set<TEntity>(), specification);
}
