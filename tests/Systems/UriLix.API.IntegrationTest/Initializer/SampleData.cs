using Microsoft.AspNetCore.Identity;
using UriLix.Domain.Entities;

namespace UriLix.API.IntegrationTest.Initializer;

internal static class SampleData
{
    internal static IEnumerable<ShortenedUrl> ShortenedURLs =>
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
    internal static IEnumerable<ApplicationUser> Users =>
    [
        new ApplicationUser()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@email.com",
            UserName = "john@email.com",
            NormalizedEmail = "JOHN@EMAIL.COM",
            PasswordHash = "Test123@"
        },
        new ApplicationUser() {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@email.com",
            NormalizedEmail = "JANE@EMAIL.COM",
            UserName = "jane@email.com",
            PasswordHash = "Test123@"
        }
    ];
    internal static IEnumerable<ApplicationUser> ApplicationUsers()
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        IEnumerable<ApplicationUser> users =
        [
            new ApplicationUser()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                UserName = "john@email.com",
                NormalizedEmail = "JOHN@EMAIL.COM",
                PasswordHash = "Test123@"
            },
            new ApplicationUser() {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@email.com",
                NormalizedEmail = "JANE@EMAIL.COM",
                UserName = "jane@email.com",
                PasswordHash = "Test123@"
            }
        ];
        foreach ( var user in users )
        {
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash!);
            yield return user;
        }
    }
}
