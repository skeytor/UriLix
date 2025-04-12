using System.Linq.Expressions;

namespace UriLix.Domain.Specifications;

/// <summary>
/// Represents a specification pattern for querying entities.
/// </summary>
/// <typeparam name="T">
/// The type of the entity to be queried.
/// </typeparam>
/// <param name="criteria">
/// Optional criteria for filtering the entities.
/// </param>
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

    /// <summary>
    /// Adds an include expression to the specification.
    /// </summary>
    /// <param name="includeExpressions">
    /// Expressions to include related entities in the query.
    /// </param>
    protected void AddIncludes(params IEnumerable<Expression<Func<T, object>>> includeExpressions) 
        => _includeExpression.AddRange(includeExpressions);
    /// <summary>
    /// Adds an order by ascending expression to the specification.
    /// </summary>
    /// <param name="orderByExpression">
    /// Order by ascending expression to sort the entities
    /// </param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
        OrderByDescendingExpression = null;
    }
    /// <summary>
    /// Adds an order by descending expression to the specification.
    /// </summary>
    /// <param name="orderByDescendingExpression">
    /// Order by descending expression to sort the entities
    /// </param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
        OrderByExpression = null;
    }
    /// <summary>
    /// Adds a search expression to the specification.
    /// </summary>
    /// <param name="searchExpression">
    /// Search expression to filter the entities
    /// </param>
    protected void AddSearch(Expression<Func<T, bool>> searchExpression)
        => SearchExpression = searchExpression;
    /// <summary>
    /// Adds pagination to the specification.
    /// </summary>
    /// <param name="skip">Page number to skip to (1 = first page)</param>
    /// <param name="take">Number of items per page</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when skip is negative or take is not positive.</exception>
    /// <remarks>
    /// Sets <see cref="IsPaginated"/> to <c>true</c> to indicate that pagination is applied.
    /// </remarks>
    protected void AddPagination(int skip, int take)
    {
        if (skip < 0 || take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip), 
                "Skip and Take values must be non-negative and Take must be greater than zero.");
        }
        Skip = (skip - 1) * take;
        Take = take;
        IsPaginated = true;
    }
}
