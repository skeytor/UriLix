using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening.Shortening;
using UriLix.Shared.Results;

namespace UriLix.API.UnitTest.Controllers;

public class UrlShorteningControllerTest
{
    private readonly DefaultHttpContext _defaultContext = new()
    {
        User = new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())]))
    };

    [Theory]
    [InlineData("https://localhost.com", "xa23s")]
    public async Task ShortenUrl_Should_GenerateShortCode_When_AliasIsNotProvided(
        string longUrl, string shortCodeExpected)
    {
        Mock<IShortenUrlService> mockService = new();
        CreateShortenUrlRequest request = new(longUrl);
        mockService.Setup(service => service.ExecuteAsync(
            It.IsAny<CreateShortenUrlRequest>(),
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(shortCodeExpected);
        LinksController sut = new(shortenUrlService: mockService.Object, default!, default!, default!, default!)
        {
            ControllerContext = new()
            {
                HttpContext = _defaultContext
            }
        };

        var result = await sut.ShortenUrlAsync(request);

        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;

        Assert.NotNull(createdAtRouteResult);
        Assert.Equal(shortCodeExpected, createdAtRouteResult.Value);
    }

    [Theory]
    [InlineData("https://longurl.com", "custom-alias")]
    public async Task ShortenUrl_Should_ReturnAlias_When_AliasIsProvided(
        string longUrl, string aliasExpected)
    {
        Mock<IShortenUrlService> mockService = new();
        CreateShortenUrlRequest request = new(longUrl);
        mockService.Setup(service => service.ExecuteAsync(
            It.IsAny<CreateShortenUrlRequest>(),
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(aliasExpected);
        LinksController sut = new(shortenUrlService: mockService.Object, default!, default!, default!, default!)
        {
            ControllerContext = new()
            {
                HttpContext = _defaultContext
            }
        };

        var result = await sut.ShortenUrlAsync(request);

        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;
        Assert.NotNull(createdAtRouteResult);
        Assert.Equal(aliasExpected, createdAtRouteResult.Value);
    }

    [Theory]
    [InlineData("url", "valid-alias")]
    public async Task ShortenUrl_Should_ReturnBadRequest_When_AliasAlreadyExists(string url, string code)
    {
        Mock<IShortenUrlService> mockService = new();
        CreateShortenUrlRequest request = new(url, code);
        var failure = Result.Failure<string>(Error.Validation("", ""));
        mockService.Setup(service => service.ExecuteAsync(
            It.IsAny<CreateShortenUrlRequest>(),
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(failure);
        LinksController sut = new(shortenUrlService: mockService.Object, default!, default!, default!, default!)
        {
            ControllerContext = new()
            {
                HttpContext = _defaultContext
            }
        };

        var result = await sut.ShortenUrlAsync(request);

        var badRequestResult = result.Result as BadRequest;
        Assert.NotNull(badRequestResult);
    }
}
