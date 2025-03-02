using System.Net.Http.Json;
using UriLix.Application.DOTs;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest.Sys.Controllers;

public class UrlShorteningControllerTest(
    IntegrationTestWebApplication<Program> _factory,
    ITestOutputHelper _outputHelper)
    : BaseWebApplicationTest(_factory, _outputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    public async Task PostShortenUrl_Should_ReturnShorCode_When_UrlIsValid(string url)
    {
        const int EXPECTED_LENGHT = 5;
        CreateShortenedUrlRequest request = new(url);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/UrlShortening", request);

        response.EnsureSuccessStatusCode();
        string? shortCode = await response.Content.ReadFromJsonAsync<string>();

        Assert.Equal(EXPECTED_LENGHT, shortCode?.Length);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "custom-alias")]
    public async Task PostShortenUrl_Should_ReturnAliasName_When_CustomAliasIsProvided(string url, string alias)
    {
        CreateShortenedUrlRequest request = new(url, Alias: alias);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/UrlShortening", request);
        response.EnsureSuccessStatusCode();
        string? aliasCreated = await response.Content.ReadFromJsonAsync<string>();

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(alias, aliasCreated);
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "invalid alias")]
    public async Task PostShortenUrl_Should_ReturnBadRequest_When_CustomAliasIsInvalid(string url, string alias)
    {
        CreateShortenedUrlRequest request = new(url, Alias: alias);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/UrlShortening", request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{message}");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("https//www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    public async Task PostShortenUrl_Should_ReturnBadRequest_When_UrlIsInvalid(string invalidUrl)
    {
        CreateShortenedUrlRequest request = new(invalidUrl);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/UrlShortening", request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{message}");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        
    }

    //[Theory]
    //[InlineData("/api/UrlShortening", "def34", "https://www.bing.com")]
    //public async Task GetOriginalUrl_Should_ReturnOriginalUrl_When_ShortCodeExists(
    //    string requestUri, string shortCode, string expectedUrl)
    //{
    //    HttpResponseMessage response = await httpClient.GetAsync($"{requestUri}/{shortCode}");

    //    outputHelper.WriteLine($"Status Code: {response.StatusCode}");
    //    outputHelper.WriteLine($"Location Header: {response.Headers.Location}");
    //    response.EnsureSuccessStatusCode();

    //    Assert.Equal(System.Net.HttpStatusCode.PermanentRedirect, response.StatusCode);
    //    Uri? locationHeader = response.Headers.Location;
    //    Assert.NotNull(locationHeader);
    //    Assert.Equal(expectedUrl, locationHeader?.AbsoluteUri);
    //}
}