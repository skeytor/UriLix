using System.Net.Http.Json;
using UriLix.Application.DOTs;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest.Sys.Controllers;

public class UrlShorteningControllerTest(
    IntegrationTestWebApplication<Program> factory,
    ITestOutputHelper outputHelper)
    : BaseWebApplicationTest(factory, outputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    public async Task POST_ShortenUrl_Should_Return201CreatedAtRoute_When_UrlIsValid(string url)
    {
        const int EXPECTED_LENGHT = 5;
        CreateShortenedUrlRequest request = new(url);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Links", request);

        response.EnsureSuccessStatusCode();
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{response.Headers.Location}");
        string? shortCode = await response.Content.ReadFromJsonAsync<string>();

        Assert.Equal(EXPECTED_LENGHT, shortCode?.Length);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "custom-alias")]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "customAlias")]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "C1stom-1alia2s")]
    public async Task POST_ShortenUrl_Should_Return201CreatedAtRoute_When_CustomAliasIsUnique(string url, string alias)
    {
        CreateShortenedUrlRequest request = new(url, alias);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Links", request);
        response.EnsureSuccessStatusCode();

        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{response.Headers.Location}");
        string? aliasCreated = await response.Content.ReadFromJsonAsync<string>();

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(alias, aliasCreated);
    }


    [Theory]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "invalid alias")]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "invalid!alias")]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "invalid@alias")]
    [InlineData("https://www.youtube.com/watch?v=191CJFrvBxM&t=923s", "invalid$alias")]
    public async Task POST_ShortenUrl_Should_Return404BadRequest_When_CustomAliasIsInvalid(string url, string invalidAlias)
    {
        CreateShortenedUrlRequest request = new(url, invalidAlias);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Links", request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{message}");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("https//www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    [InlineData("https:/www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    [InlineData("invalid-url")]
    public async Task POST_ShortenUrl_Should_Return404BadRequest_When_UrlIsInvalid(string invalidUrl)
    {
        CreateShortenedUrlRequest request = new(invalidUrl);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Links", request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{message}");
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        
    }

    [Theory]
    [InlineData("/api/Links", "def34", "https://www.bing.com/")]
    public async Task GET_ResolveUrl_Should_ReturnOriginalUrl_When_ShortCodeExists(
        string uri, string shortCode, string expectedUrl)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{uri}/{shortCode}");

        outputHelper.WriteLine($"Status Code: {response.StatusCode}");
        outputHelper.WriteLine($"Location Header: {response.Headers.Location}");

        Assert.Equal(System.Net.HttpStatusCode.Found, response.StatusCode);
        Uri location = Assert.IsType<Uri>(response.Headers.Location);
        Assert.NotNull(location);
        Assert.Equal(expectedUrl, location.AbsoluteUri);
    }
}