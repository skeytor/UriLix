using System.Threading.Tasks;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Repositories;
using UriLix.Persistence.Test.Fixtures;
using Xunit.Abstractions;

namespace UriLix.Persistence.Test.Repositories;

public class ShortenedUrlRepositoryTest(
    DatabaseFixture fixture,
    ITestOutputHelper outputHelper) : TestBase(fixture, outputHelper)
{
    // [ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public async Task Insert_Should_ReturnShortedUrlEntity_WhenDataIsValid()
    {
        await ExecutedInATransactionAsync(RunTest);
        async Task RunTest()
        {
            //DateTime createdAt = DateTime.Now;
            ShortenedLink shortenedUrl = new()
            {
                OriginalUrl = "https://original.com",
                Alias = "xl1",
                ShortCode = "https://short.com",
            };
            ShortenedLinkRepository repository = new(context);

            var result = await repository.InsertAsync(shortenedUrl);
            await context.SaveChangesAsync();

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.IsType<ShortenedLink>(result);
            Assert.NotNull(shortenedUrl);
        }
    }
    [Fact]
    public async Task Find_Should_ReturnShortedUrl_WhenRecordExists()
    {
        async Task RunTest()
        {
            var shortenedLink
        }
    }
}
