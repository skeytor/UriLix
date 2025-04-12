using System.Linq.Expressions;

namespace UriLix.Domain.Specifications;

public abstract class Specification<T>(Expression<Func<T, bool>>? criteria = null) where T : class
{
    private readonly List<Expression<Func<T, object>>> _includeExpression = [];
    public Expression<Func<T, bool>>? Criteria { get; } = criteria;
    public IReadOnlyList<Expression<Func<T, object>>> IncludeExpression => _includeExpression;
    public Expression<Func<T, object>>? OrderByExpression { get; private set; }
    public Expression<Func<T, object>>? OrderByDescendingExpression { get; private set; }
    public Expression<Func<T, bool>>? SearchExpression {  get; private set; }
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPaginated { get; private set; }

    protected void AddIncludes(params IEnumerable<Expression<Func<T, object>>> includeExpressions) 
        => _includeExpression.AddRange(includeExpressions);
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
        OrderByDescendingExpression = null;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
        OrderByExpression = null;
    }
    protected void AddSearch(Expression<Func<T, bool>> searchExpression) => SearchExpression = searchExpression;
    protected void AddPagination(int skip, int take)
    {
        if (skip < 0 || take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), "Skip and Take values must be non-negative and Take must be greater than zero.");
        }
        Skip = (skip - 1) * take;
        Take = take;
        IsPaginated = true;
    }
}
