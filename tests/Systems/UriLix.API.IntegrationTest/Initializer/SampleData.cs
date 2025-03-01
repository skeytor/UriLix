using UriLix.Domain.Entities;

namespace UriLix.API.IntegrationTest.Initializer;

internal static class SampleData
{
    internal static List<ShortenedUrl> ShortenedURLs =>
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
    internal static IEnumerable<User> Users =>
    [
        new User()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@email.com",
            Password = "Test123"
        },
        new User() {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@email.com",
            Password = "Test123"
        }
    ];
}
