using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.UnitTest.Controllers;

public class UrlShorteningControllerTest
{
    [Theory]
    [InlineData("https://localhost.com", "xa23s")]
    public async Task ShortenUrl_Should_ReturnShortCode_When_ShortCodeIsNotNullAndAliasIsNull(
        string longUrl, string shortCodeExpected)
    {
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new(longUrl);
        CreateShortenedUrlResponse response = new(shortCodeExpected, FilterType.ShortCode);
        mockService.Setup(service => service.ShortenUrlAsync(It.IsAny<CreateShortenedUrlRequest>()))
            .ReturnsAsync(response);
        UrlShorteningController sut = new(mockService.Object);

        var result = await sut.ShortenUrl(request);
        
        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;

        Assert.NotNull(createdAtRouteResult);
        Assert.Equal(shortCodeExpected, createdAtRouteResult.Value);
    }

    [Theory]
    [InlineData("https://longurl.com", "custom-alias")]
    public async Task ShortenUrl_Should_ReturnAlias_When_AliasIsNotNullAndShortCodeIsNull(
        string longUrl, string aliasExpected)
    {
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new(longUrl);
        CreateShortenedUrlResponse response = new(aliasExpected, FilterType.Alias);
        mockService.Setup(service => service.ShortenUrlAsync(It.IsAny<CreateShortenedUrlRequest>()))
            .ReturnsAsync(response);
        UrlShorteningController sut = new(mockService.Object);

        var result = await sut.ShortenUrl(request);

        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;

        Assert.NotNull(createdAtRouteResult);
        Assert.Equal(aliasExpected, createdAtRouteResult.Value);
    }

    [Fact]
    public async Task ShortenUrl_Should_ReturnCreatedAtRoute_When_ShortCodeIsProvided()
    {
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new("url");
        CreateShortenedUrlResponse response = new("xase1", FilterType.ShortCode);
        mockService.Setup(service => service.ShortenUrlAsync(It.IsAny<CreateShortenedUrlRequest>()))
            .ReturnsAsync(response);
        UrlShorteningController sut = new(mockService.Object);

        var result = await sut.ShortenUrl(request);

        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;

        Assert.NotNull(createdAtRouteResult);
        HashSet<string> routeKeys = [.. createdAtRouteResult.RouteValues.Keys];
        List<string> keys = ["Code", "Type"];
        Assert.Collection(routeKeys, key => keys.Contains(key, Equ);
    }
}
