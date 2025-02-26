using UriLix.Domain.Entities;

namespace UriLix.API.IntegrationTest.Initializer;

internal static class SampleData
{
    internal static List<ShortenedUrl> ShortenedUrls =>
    [
        new ShortenedUrl()
        {
            OriginalUrl = "https://www.google.com",
            ShortCode = "abc12",
        },
        new ShortenedUrl()
        {
            OriginalUrl = "https://www.bing.com",
            ShortCode = "def34",
        },
        new ShortenedUrl()
        {
            OriginalUrl = "https://www.yahoo.com",
            ShortCode = "ghi56",
        },
        new ShortenedUrl()
        {
            OriginalUrl = "https://www.duckduckgo.com",
            ShortCode = "jkl78",
        },
        new ShortenedUrl()
        {
            OriginalUrl = "https://www.ecosia.com",
            ShortCode = "mno90",
        }
    ];
}
