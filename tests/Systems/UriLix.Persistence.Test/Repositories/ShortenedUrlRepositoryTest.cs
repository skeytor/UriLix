using UriLix.Domain.Entities;
using UriLix.Persistence.Repositories;
using UriLix.Persistence.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace UriLix.Persistence.IntegrationTest.Repositories;

public class ShortenedUrlRepositoryTest(
    DatabaseFixture fixture,
    ITestOutputHelper outputHelper) : TestBase(fixture, outputHelper)
{
    //[ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public async Task Insert_Should_ReturnShortedUrlEntity_WhenDataIsValid()
    {
        await ExecutedInATransactionAsync(RunTest);
        async Task RunTest()
        {
            ShortenedUrl shortenedUrl = new()
            {
                OriginalUrl = "https://original.com",
                Alias = "xl1",
                ShortCode = "https://short.com",
            };
            ShortenedUrlRepository repository = new(context);

            var result = await repository.InsertAsync(shortenedUrl);
            await context.SaveChangesAsync();

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.IsType<ShortenedUrl>(result);
            Assert.NotNull(shortenedUrl);
        }
    }
    
    [Fact]
    public async Task FindById_Should_ReturnShortedUrl_WhenRecordExistsWithoutIncludes()
    {
        await ExecutedInATransactionAsync(RunTest);
        async Task RunTest()
        {
            Guid id = context.ShortenedUrl.First().Id;
            ShortenedUrlRepository repository = new(context);
            
            var result = await repository.FindByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }
    }

    [Fact]
    public async Task FindById_Should_ReturnShortedUrl_WhenRecordExistsWithIncludes()
    {
        await ExecutedInATransactionAsync(RunTest);
        async Task RunTest()
        {
            Guid id = context.ShortenedUrl.First().Id;
            ShortenedUrlRepository repository = new(context);

            var result = await repository.FindByIdAsync(id, includes: x => x.User);

            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(id, result.Id);
        }
    }
}
