using Microsoft.EntityFrameworkCore;
using UriLix.Domain.Specifications;

namespace UriLix.Persistence.Specifications;

internal static class SpecificationEvaluator
{
    internal static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T> specification) 
        where T : class
    {
        IQueryable<T> query = inputQuery;
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        if (specification.SearchExpression is not null)
        {
            query = query.Where(specification.SearchExpression);
        }
        query = specification.IncludeExpression
            .Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
        if (specification.OrderByExpression is not null)
        {
            query = query.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            query = query.OrderByDescending(specification.OrderByDescendingExpression);
        }
        if (specification.IsPaginated)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }
        return query;
    }
}
