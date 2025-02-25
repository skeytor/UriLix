using Bogus;
using UriLix.Domain.Entities;

namespace UriLix.Persistence.IntegrationTest.Initializers;

internal static class DataInitializer
{
    internal static async Task SeedData(AppDbContext context, CancellationToken cancellationToken)
    {
        Faker<ShortenedUrl> shortenedLinkFaker = new Faker<ShortenedUrl>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.OriginalUrl, f => f.Internet.Url())
            .RuleFor(x => x.ShortCode, f => f.Random.Hash(6))
            .RuleFor(x => x.Alias, f => f.Random.Hash(4));

        Faker<User> userFaker = new Faker<User>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .RuleFor(x => x.ShortenedLinks, f => [.. shortenedLinkFaker.Generate(3)]);
        
        List<ShortenedUrl> links = shortenedLinkFaker.Generate(10);
        List<User> users = userFaker.Generate(10);
        
        await context.Users.AddRangeAsync(users, cancellationToken);
        await context.ShortenedUrl.AddRangeAsync(links, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
