using Microsoft.AspNetCore.Http;
using System.Net;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.ClickStatistics;

public class ClickTrackingService(
    IClickTrackingRepository repository,
    IUnitOfWork unitOfWork) : IClickTrackingService
{
    public Task<Result> GetStatisticsForUrlAsync(string code)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> RecordClickAsync(ShortenedUrl shortenedUrl, HttpRequest request)
    {
        string userAgent = request.Headers["User-Agent"].ToString();
        string referer = request.Headers["Referer"].ToString() ?? string.Empty;
        ClickStatistic clickStatistic = new()
        {
            ShortenedUrlId = shortenedUrl.Id,
            Device = "mobile",
            Browser = "browser",
            UserAgent = userAgent,
            Referer = referer,
            VisitedAt = DateTime.UtcNow
        };
        await repository.InsertAsync(clickStatistic);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
