namespace UriLix.Shared.Pagination;

public class PagedResult<T> where T : class
{
    public IReadOnlyList<T> Items { get; } = [];
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
    private PagedResult(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    public static PagedResult<T> Create(IReadOnlyList<T> items, int page, int pageSize, int totalCount) 
        => new(items, page, pageSize, totalCount);
}
