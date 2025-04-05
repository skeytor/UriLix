using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;
using UriLix.Shared.Results;

namespace UriLix.API.UnitTest.Controllers;

public class UrlShorteningControllerTest
{
    [Theory]
    [InlineData("https://localhost.com", "xa23s")]
    public async Task ShortenUrl_Should_GenerateShortCode_When_AliasIsNotProvided(
        string longUrl, string shortCodeExpected)
    {
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new(longUrl);
        DefaultHttpContext defaultHttpContext = new()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "test-user-id")
            ]))
        };
        mockService.Setup(service => service.ShortenUrlAsync(It.IsAny<CreateShortenedUrlRequest>(),
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(shortCodeExpected);
        LinksController sut = new(mockService.Object)
        {
            ControllerContext = new()
            {
                HttpContext = defaultHttpContext
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
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new(longUrl);
        DefaultHttpContext defaultHttpContext = new()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "test-user-id")
            ]))
        };
        mockService.Setup(service => service.ShortenUrlAsync(
            It.IsAny<CreateShortenedUrlRequest>(), 
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(aliasExpected);
        LinksController sut = new(mockService.Object)
        {
            ControllerContext = new()
            {
                HttpContext = defaultHttpContext
            }
        };

        var result = await sut.ShortenUrlAsync(request);

        var createdAtRouteResult = result.Result as CreatedAtRoute<string>;
        Assert.NotNull(createdAtRouteResult);
        Assert.Equal(aliasExpected, createdAtRouteResult.Value);
    }

    [Fact]
    public async Task ShortenUrl_Should_ReturnBadRequest_When_AliasAlreadyExists()
    {
        Mock<IUrlShorteningService> mockService = new();
        CreateShortenedUrlRequest request = new("url", Alias: "alias-exists");
        var failure = Result.Failure<string>(Error.Validation("", ""));
        mockService.Setup(service => service.ShortenUrlAsync(
            It.IsAny<CreateShortenedUrlRequest>(),
            It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(failure);
        LinksController sut = new(mockService.Object)
        {
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await sut.ShortenUrlAsync(request);

        var badRequestResult = result.Result as BadRequest;
        Assert.NotNull(badRequestResult);
    }
}
