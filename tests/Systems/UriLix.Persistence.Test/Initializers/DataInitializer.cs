using Bogus;
using UriLix.Domain.Entities;

namespace UriLix.Persistence.IntegrationTest.Initializers;

internal static class DataInitializer
{
    internal static async Task SeedData(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        Faker<ShortenedUrl> shortenedLinkFaker = new Faker<ShortenedUrl>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.OriginalUrl, f => f.Internet.Url())
            .RuleFor(x => x.ShortCode, f => f.Random.Hash(4));

        Faker<ApplicationUser> userFaker = new Faker<ApplicationUser>()
            .RuleFor(x => x.Id, f => f.Random.Guid().ToString())
            .RuleFor(x => x.FirstName, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.PasswordHash, f => f.Internet.Password())
            .RuleFor(x => x.ShortenedURLs, f => [.. shortenedLinkFaker.Generate(3)]);
        
        List<ShortenedUrl> links = shortenedLinkFaker.Generate(10);
        List<ApplicationUser> users = userFaker.Generate(10);
        
        await context.Users.AddRangeAsync(users, cancellationToken);
        await context.ShortenedUrl.AddRangeAsync(links, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
