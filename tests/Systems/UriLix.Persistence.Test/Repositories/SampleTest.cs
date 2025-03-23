using UriLix.Domain.Entities;
using UriLix.Persistence.IntegrationTest.Fixtures;
using Xunit.Abstractions;

namespace UriLix.Persistence.IntegrationTest.Repositories;

public class SampleTest(DatabaseFixture fixture, ITestOutputHelper outputHelper) 
    : TestBase(fixture, outputHelper)
{
    // [ThingUnderTest}_Should_[ExpectedResult]_[Conditions]
    [Fact]
    public void Do_Should_MakeSomething()
    {
        ExecutedInATransaction(RunTest);
        void RunTest()
        {
            ApplicationUser user = new()
            {
                FirstName = "Test",
                Email = "test",
                PasswordHash = "test",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                LastLoginAt = DateTime.Now,
            };
            context.Users.Add(user);
            context.SaveChanges();
            Assert.True(true);
        }
    }
}
