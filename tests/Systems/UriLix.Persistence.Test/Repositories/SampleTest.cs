﻿using UriLix.Domain.Entities;
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
            User user = new()
            {
                FirstName = "Test",
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
    }
}
