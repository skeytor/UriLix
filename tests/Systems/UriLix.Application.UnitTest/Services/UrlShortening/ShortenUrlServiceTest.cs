using Microsoft.Extensions.Caching.Hybrid;
using Moq;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Application.Services.UrlShortening.Shortening;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Enums;
using UriLix.Shared.UnitOfWork;
using Xunit.Abstractions;

namespace UriLix.Application.UnitTest.Services.UrlShortening;

public class ShortenUrlServiceTest(ITestOutputHelper testOutputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData("https://local.com", "abc123")]
    [InlineData("https://local.com", "abs311")]
    public async Task ShortenUrlAsync_Should_ReturnShortCode_When_UrlIsValid(
        string url, string shortCodeExpected)
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<HybridCache> mockCache = new();
        mockProvider
            .Setup(x => x.GenerateShortCode())
            .Returns(shortCodeExpected);
        CreateShortenUrlRequest request = new(url);
        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);

        var result = await sut.ExecuteAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(shortCodeExpected, result.Value);

        mockRepo.Verify(x => x.InsertAsync(It.IsAny<ShortenedUrl>()), Times.Once);
        mockUnit.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [InlineData("url-invalid")]
    [InlineData("http:/url-invalid")]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_UrlIsInvalid(string invalidUrl)
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<HybridCache> mockCache = new();
        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);
        CreateShortenUrlRequest request = new(invalidUrl);

        var result = await sut.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal("Url.Invalid", result.Error.Code);
    }

    [Theory]
    [InlineData("https://localhost.com", "adf12", "abc123")]
    public async Task ShortenUrlAsync_Should_Retry_When_ShortCodeGeneratedIsDuplicated(
        string url, string invalidCode, string shortCodeExpected)
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<HybridCache> mockCache = new();
        mockRepo.SetupSequence(x => x.ShortUrlExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true) // First attempt: short code exists
            .ReturnsAsync(false); // Second attempt: short code is unique
        mockProvider
            .SetupSequence(x => x.GenerateShortCode())
            .Returns(invalidCode) // First attempt
            .Returns(shortCodeExpected); // Second attempt
        CreateShortenUrlRequest request = new(url);
        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);

        var result = await sut.ExecuteAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(shortCodeExpected, result.Value);

        mockRepo.Verify(x => x.ShortUrlExistsAsync(It.IsAny<string>()), Times.Exactly(2));
        mockProvider.Verify(x => x.GenerateShortCode(), Times.Exactly(2));
    }

    [Theory]
    [InlineData("https://localhost.com")]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_AllShortCodesAreDuplicated(string url)
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<HybridCache> mockCache = new();

        // Simulate all short codes being duplicates
        mockRepo.Setup(x => x.ShortUrlExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        mockProvider.Setup(x => x.GenerateShortCode())
            .Returns(string.Empty);

        CreateShortenUrlRequest request = new(url);
        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);

        var result = await sut.ExecuteAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
        Assert.Equal("ShortCode.Duplicate", result.Error.Code);
        testOutputHelper.WriteLine($"{result.Error.Description}");
    }

    [Fact]
    public async Task ShortenUrlAsync_Should_ReturnAlias_When_CustomAliasIsProvided()
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<HybridCache> mockCache = new();

        string url = "https://localhost.com";
        string aliasExpected = "my-custom-alias";
        CreateShortenUrlRequest request = new(url, Alias: aliasExpected);

        mockRepo.Setup(x => x.ShortUrlExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);

        var result = await sut.ExecuteAsync(request);
        testOutputHelper.WriteLine($"Response:\n\t Custom Alias {result.Value}");

        Assert.True(result.IsSuccess);
        Assert.Equal(aliasExpected, result.Value);

        mockRepo.Verify(x => x.InsertAsync(It.IsAny<ShortenedUrl>()), Times.Once);
        mockUnit.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_CustomAliasAlreadyExists()
    {
        Mock<IShortenedUrlRepository> mockRepo = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<HybridCache> mockCache = new();
        string url = "https://localhost.com";
        string alias = "my-custom-alias";
        CreateShortenUrlRequest request = new(url, Alias: alias);
        mockRepo.Setup(x => x.ShortUrlExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        ShortenUrlService sut = new(mockRepo.Object, mockProvider.Object, mockCache.Object, mockUnit.Object);

        var result = await sut.ExecuteAsync(request);
        testOutputHelper.WriteLine($"Error:\n\t {result.Error.Description}");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
        Assert.Equal("Alias.Duplicate", result.Error.Code);
    }
}
