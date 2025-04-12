using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Specifications;
using UriLix.Shared.Pagination;

namespace UriLix.Persistence.Specifications;

internal class GetAllByUserIdSpec : Specification<ShortenedUrl>
{
    internal GetAllByUserIdSpec(string userId, PaginationQuery paginationQuery) 
        : base(x => x.UserId == userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(paginationQuery);

        AddIncludes(x => x.ClickStatistics);
        AddPagination(paginationQuery.Page, paginationQuery.PageSize);
        if (!string.IsNullOrWhiteSpace(paginationQuery.SearchTerm))
        {
            AddSearch(x => x.ShortCode.Contains(paginationQuery.SearchTerm) ||
                           x.OriginalUrl.Contains(paginationQuery.SearchTerm));
        }
        Expression<Func<ShortenedUrl, object>> keySelector = GetSortProperty(paginationQuery);
        if (string.Equals(paginationQuery.SortOrder, "desc", StringComparison.OrdinalIgnoreCase))
        {
            AddOrderByDescending(keySelector);
        }
        else
        {
            AddOrderBy(keySelector);
        }
    }
    private static Expression<Func<ShortenedUrl, object>> GetSortProperty(PaginationQuery paginationQuery) 
        => paginationQuery.SortColumn?.ToLower() switch
        {
            "code" => x => x.ShortCode,
            "url" => x => x.OriginalUrl,
            "createdAt" => x => x.CreateAt,
            _ => x => x.Id,
        };
}
