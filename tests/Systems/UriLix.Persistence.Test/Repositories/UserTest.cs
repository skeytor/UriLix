using UriLix.Domain.Entities;
using UriLix.Persistence.Test.Fixtures;
using Xunit.Abstractions;

namespace UriLix.Persistence.Test.Repositories;

public class UserTest(DatabaseFixture fixture, ITestOutputHelper outputHelper) 
    : TestBase(fixture, outputHelper)
{
    // [ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public async Task Do_Should_MakeSomething()
    {
        testOutputHelper.WriteLine($"===== EXECUTING THE FIRST TEST CLASE AT ====== {DateTime.Now}");
        ExecutedInATransaction(RunTest);
        void RunTest()
        {
            User user = new()
            {
                UserName = "Test",
                Email = "test",
                Password = "test",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                LastLoginAt = DateTime.Now,
            };
            context.Users.Add(user);
            context.SaveChanges();
            Assert.NotEqual(Guid.Empty, user.Id);
        }
        testOutputHelper.WriteLine($"===== FINISHING THE FIRST TEST CLASE AT ====== {DateTime.Now}");

    }
    [Fact]
    public async Task Do_Should_MakeSomethingMore()
    {
        testOutputHelper.WriteLine($"===== EXECUTING THE SECOND TEST CLASE AT ====== {DateTime.Now}");

        ExecutedInATransaction(RunTest);
        void RunTest()
        {
            // Here I wanna print the query string
            var users = context.ShortenedUrl.ToList();
            Assert.True(true);
        }
        testOutputHelper.WriteLine($"===== FINISHING THE SECOND TEST CLASE AT ====== {DateTime.Now}");
    }
}
