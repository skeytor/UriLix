using Microsoft.EntityFrameworkCore;
using UriLix.Persistence.Test.Initializers;
using Xunit.Abstractions;

namespace UriLix.Persistence.Test.Helpers;

internal static class TestHelper
{
    private static DbContextOptions<AppDbContext> GetMsSQLDbOptions(
        string connectionString, 
        ITestOutputHelper testOutput)
        => new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .UseAsyncSeeding(async (context, _, cancellationToken) => await DataInitializer.SeedData(context, cancellationToken))
            .LogTo(testOutput.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .Options;
    internal static AppDbContext GetAppDbContext(string connectionString, ITestOutputHelper testOutput) 
        => new(GetMsSQLDbOptions(connectionString, testOutput));
}
