using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UriLix.Persistence.Test.Fixtures;
using UriLix.Persistence.Test.Helpers;
using Xunit.Abstractions;

namespace UriLix.Persistence.Test;

[Collection(nameof(DatabaseCollection))]
public abstract class TestBase(DatabaseFixture fixture, ITestOutputHelper outputHelper) : IAsyncLifetime
{
    protected readonly ITestOutputHelper testOutputHelper = outputHelper;
    protected readonly AppDbContext context = TestHelper.GetAppDbContext(fixture.ConnectionString, outputHelper);
    public async Task DisposeAsync() => await context.DisposeAsync();

    public async Task InitializeAsync()
    {
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }

    protected void ExecutedInATransaction(Action action)
    {
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using IDbContextTransaction transaction = context.Database.BeginTransaction();
            action();
            transaction.Rollback();
        });
    }
    protected async Task ExecutedInATransactionAsync(Func<Task> action)
    {
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            await action();
            await transaction.RollbackAsync();
        });
    }
}