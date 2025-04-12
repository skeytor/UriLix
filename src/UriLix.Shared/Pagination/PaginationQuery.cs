using System.ComponentModel.DataAnnotations;

namespace UriLix.Shared.Pagination;

public sealed record PaginationQuery(
    string? SearchTerm,

    [AllowedValues(null,"code", "url", "createdAt", ErrorMessage = "Sort column must be either 'code', 'url', or 'createdAt'")]
    string? SortColumn,

    [AllowedValues(null,"asc", "desc", ErrorMessage = "Sort order must be either 'asc' or 'desc'")]
    string? SortOrder,

    int Page = 1,
    int PageSize = 10)
{
    private const int MAX_PAGE_SIZE = 50;
    public int PageSize { get; init; } = PageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : PageSize;
};
