using Moq;
using System.Linq.Expressions;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Application.Services.UrlShortening;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Enums;
using UriLix.Shared.UnitOfWork;
using Xunit.Abstractions;

namespace UriLix.Application.UnitTest.Services;

public class UrlShortingServiceTest(ITestOutputHelper testOutputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData("https://local.com", "abc123")]
    [InlineData("https://local.com", "abs311")]
    public async Task ShortenUrlAsync_Should_ReturnShortCode_When_UrlIsValid(
        string url, string shortCodeExpected)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        mockProvider
            .Setup(x => x.GenerateShortCode())
            .Returns(shortCodeExpected);
        CreateShortenedUrlRequest request = new(url);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.ShortenUrlAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(shortCodeExpected, result.Value.Code);

        mockRepository.Verify(x => x.InsertAsync(It.IsAny<ShortenedUrl>()), Times.Once);
        mockUnit.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [InlineData("url-invalid")]
    [InlineData("http:/url-invalid")]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_UrlIsInvalid(string invalidUrl)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        UrlShorteningService sut = new(mockRepository.Object, mockProvider.Object, mockUnitOfWork.Object);
        CreateShortenedUrlRequest request = new(invalidUrl);

        var result = await sut.ShortenUrlAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal("Url.Invalid", result.Error.Code);
    }

    [Theory]
    [InlineData("https://localhost.com","adf12", "abc123")]
    public async Task ShortenUrlAsync_Should_Retry_When_ShortCodeGeneratedIsDuplicated(
        string url, string invalidCode, string shortCodeExpected)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        mockRepository.SetupSequence(x => x.ShortCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true) // First attempt: short code exists
            .ReturnsAsync(false); // Second attempt: short code is unique
        mockProvider
            .SetupSequence(x => x.GenerateShortCode())
            .Returns(invalidCode) // First attempt
            .Returns(shortCodeExpected); // Second attempt
        CreateShortenedUrlRequest request = new(url);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);
        
        var result = await sut.ShortenUrlAsync(request);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(shortCodeExpected, result.Value.Code);
        
        mockRepository.Verify(x => x.ShortCodeExistsAsync(It.IsAny<string>()), Times.Exactly(2));
        mockProvider.Verify(x => x.GenerateShortCode(), Times.Exactly(2));
    }

    [Theory]
    [InlineData("https://localhost.com")]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_AllShortCodesAreDuplicated(string url)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

       // Simulate all short codes being duplicates
        mockRepository.Setup(x => x.ShortCodeExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        mockProvider.Setup(x => x.GenerateShortCode())
            .Returns(string.Empty);

        CreateShortenedUrlRequest request = new(url);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.ShortenUrlAsync(request);
        
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("ShortCode.Duplicate", result.Error.Code);
        testOutputHelper.WriteLine($"{result.Error.Description}");
    }

    [Fact]
    public async Task ShortenUrlAsync_Should_ReturnAlias_When_CustomAliasIsProvided()
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

        string url = "https://localhost.com";
        string aliasExpected = "my-custom-alias";
        CreateShortenedUrlRequest request = new(url, Alias: aliasExpected);

        mockRepository.Setup(x => x.AliasExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.ShortenUrlAsync(request);
        testOutputHelper.WriteLine($"Response:\n\t Custom Alias {result.Value}");

        Assert.True(result.IsSuccess);
        Assert.Equal(aliasExpected, result.Value.Code);

        mockRepository.Verify(x => x.InsertAsync(It.IsAny<ShortenedUrl>()), Times.Once);
        mockUnit.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ShortenUrlAsync_Should_ReturnFailure_When_CustomAliasAlreadyExists()
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();
        string url = "https://localhost.com";
        string alias = "my-custom-alias";
        CreateShortenedUrlRequest request = new(url, Alias: alias);
        mockRepository.Setup(x => x.AliasExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.ShortenUrlAsync(request);
        testOutputHelper.WriteLine($"Error:\n\t {result.Error.Description}");  

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("Alias.Duplicate", result.Error.Code);
    }

    [Theory]
    [InlineData("custom-alias", "https://localhost.com/")]
    public async Task GetOriginalUrlAsync_Should_ReturnOriginalUrl_When_AliasExists(
        string alias, 
        string urlExpected)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

        OriginalUrlQueryParam queryParams = new(alias, UrlQueryType.Alias);
        mockRepository.Setup(x => x.GetOriginalUrlByAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .ReturnsAsync(urlExpected);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.GetOriginalUrlAsync(queryParams);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(urlExpected, result.Value);
    }

    [Theory]
    [InlineData("alias-not-exists")]
    public async Task GetOriginalUrlAsync_Should_ReturnFailure_When_AliasNotExist(string alias)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

        OriginalUrlQueryParam queryParams = new(alias, UrlQueryType.Alias);
        mockRepository.Setup(repo => repo.GetOriginalUrlByAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns(Task.FromResult<string?>(null));
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.GetOriginalUrlAsync(queryParams);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("Url.NotFound", result.Error.Code);
    }

    [Theory]
    [InlineData("xas12", "https://localhost.com/")]
    public async Task GetOriginalUrlAsync_Should_ReturnOriginalUrl_When_ShortCodeExist(
    string shortCode,
    string urlExpected)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

        OriginalUrlQueryParam queryParams = new(shortCode, UrlQueryType.ShortCode);
        mockRepository.Setup(x => x.GetOriginalUrlByAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .ReturnsAsync(urlExpected);
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.GetOriginalUrlAsync(queryParams);

        Assert.True(result.IsSuccess);
        Assert.Equal(urlExpected, result.Value);
    }

    [Theory]
    [InlineData("fail1")]
    public async Task GetOriginalUrlAsync_Should_ReturnFailure_When_ShortCodeNotExist(string shortCode)
    {
        Mock<IShortenedUrlRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnit = new();
        Mock<IUrlShortingProvider> mockProvider = new();

        OriginalUrlQueryParam queryParams = new(shortCode, UrlQueryType.ShortCode);
        mockRepository.Setup(repo => repo.GetOriginalUrlByAsync(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns(Task.FromResult<string?>(null));
        UrlShorteningService sut = new(
            mockRepository.Object,
            mockProvider.Object,
            mockUnit.Object);

        var result = await sut.GetOriginalUrlAsync(queryParams);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("Url.NotFound", result.Error.Code);
    }
}
