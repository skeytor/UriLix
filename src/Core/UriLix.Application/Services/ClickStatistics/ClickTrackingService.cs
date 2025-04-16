using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.ClickStatistics;

public class ClickTrackingService(
    IClickTrackingRepository repository,
    IUnitOfWork unitOfWork) : IClickTrackingService
{
    public async Task<Result> RecordClickAsync(ShortenedUrl shortenedUrl, IHeaderDictionary headersInfo)
    {
        ClickStatistic clickStatistic = new()
        {
            ShortenedUrlId = shortenedUrl.Id,
            Device = "mobile",
            Browser = "Chrome",
            UserAgent = headersInfo["User-Agent"].ToString(),
            Referer = headersInfo["Referrer"].ToString(),
            VisitedAt = DateTime.UtcNow
        };
        await repository.InsertAsync(clickStatistic);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
