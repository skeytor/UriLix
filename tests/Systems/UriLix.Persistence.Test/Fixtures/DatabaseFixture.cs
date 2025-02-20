
using Testcontainers.MsSql;

namespace UriLix.Persistence.Test.Fixtures;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    public string ConnectionString => _container.GetConnectionString();
    public Task DisposeAsync() => _container.StopAsync();

    public Task InitializeAsync() => _container.StartAsync();
}
