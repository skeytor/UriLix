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
    [InlineData("/api/UrlShortening", "https://www.youtube.com/watch?v=191CJFrvBxM&t=923s")]
    public async Task PostShortenUrl_Should_ReturnShorCode_WhenUrlIsValid(string path, string originalUrl)
    {
        CreateShortenedUrlRequest request = new(originalUrl);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(path, request);
        response.EnsureSuccessStatusCode();

        Dictionary<string, string>? data = await response.Content
            .ReadFromJsonAsync<Dictionary<string, string>>();
        string shortCode = data!["shortCode"];
        
        outputHelper.WriteLine($"Response from {httpClient.BaseAddress}: {shortCode}");
        
        Assert.Equal(5, shortCode.Length);
    }

    [Theory]
    [InlineData("def34", "https://www.bing.com")]
    public async Task GetOriginalUrl_Should_ReturnOriginalUrl_When_ShortCodeExists(string shortCode, string url)
    {

        HttpResponseMessage response = await httpClient.GetAsync($"/api/UrlShortening/{shortCode}");
        response.EnsureSuccessStatusCode();

        string originalUrl = await response.Content.ReadAsStringAsync();

        Assert.Equal(url, originalUrl);
    }
}
