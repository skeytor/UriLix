using Microsoft.AspNetCore.Http;
using UriLix.Domain.Entities;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.ClickStatistics;

public interface IClickTrackingService
{
    Task<Result> RecordClickAsync(ShortenedUrl shortenedUrl, IHeaderDictionary headersInfo);
}
