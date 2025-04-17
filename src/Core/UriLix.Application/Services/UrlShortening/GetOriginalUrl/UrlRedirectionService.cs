using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Hybrid;
using System.Net.Http.Headers;
using UriLix.Application.Services.ClickStatistics;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.GetOriginalUrl;

public class UrlRedirectionService(
    IShortenedUrlRepository repository,
    IClickTrackingService clickTrackingService,
    HybridCache hybridCache) : IUrlRedirectionService
{
    public async Task<Result<string>> ExecuteAsync(string code, IHeaderDictionary headersInfo)
    {
        ShortenedUrl? url = await hybridCache.GetOrCreateAsync(code, async entry =>
        {
            return await repository.FindByShortCodeAsync(code);
        },
        tags: ["codes"]);
        if (url is null)
        {
            return Result.Failure<string>(Error.NotFound(
            "Url.NotFound",
                $"The URL with alias: {code} was not found"));
        }
        _ = Task.Run(async () => await clickTrackingService.RecordClickAsync(url, headersInfo));
        return url.OriginalUrl;
    }
}
