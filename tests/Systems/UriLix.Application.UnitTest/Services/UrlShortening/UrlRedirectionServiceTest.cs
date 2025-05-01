using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Moq;
using UriLix.Application.Services.ClickStatistics;
using UriLix.Application.Services.UrlShortening.GetOriginalUrl;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Enums;

namespace UriLix.Application.UnitTest.Services.UrlShortening;

public class UrlRedirectionServiceTest
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData("custom-alias", "https://localhost.com/")]
    public async Task GetOriginalUrlAsync_Should_ReturnOriginalUrl_When_AliasExists(
        string alias,
        string urlExpected)
    {
        ShortenedUrl shortenedUrl = new()
        {
            Id = Guid.NewGuid(),
            OriginalUrl = urlExpected,
            ShortCode = alias
        };
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IClickTrackingService> mockTrackingService = new();
        Mock<HybridCache> mockHybridCache = new();

        mockHybridCache.Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<CancellationToken, ValueTask<ShortenedUrl>>>(),
                It.IsAny<HybridCacheEntryOptions>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((string key,
                         Func<CancellationToken, ValueTask<ShortenedUrl>> factory,
                         HybridCacheEntryOptions options,
                         IEnumerable<string> tags,
                         CancellationToken token) =>
            {
                return shortenedUrl;
            });


        mockRepo.Setup(repo => repo.GetOriginalUrlAsync(It.IsAny<string>()))
            .ReturnsAsync(urlExpected);
        ResolveUrlService sut = new(mockRepo.Object, mockTrackingService.Object, mockHybridCache.Object);

        var result = await sut.ExecuteAsync(alias, default!);

        Assert.True(result.IsSuccess);
        Assert.Equal(urlExpected, result.Value);
    }

    [Theory]
    [InlineData("alias-not-exists")]
    public async Task GetOriginalUrlAsync_Should_ReturnFailure_When_AliasNotExist(string alias)
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IClickTrackingService> mockTrackingService = new();
        Mock<HybridCache> mockHybridCache = new();
        mockRepo.Setup(repo => repo.GetOriginalUrlAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<string?>(null));

        ResolveUrlService sut = new(mockRepo.Object, mockTrackingService.Object, mockHybridCache.Object);

        var result = await sut.ExecuteAsync(alias, default!);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("Url.NotFound", result.Error.Code);
    }
}
