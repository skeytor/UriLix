using Microsoft.EntityFrameworkCore;
using UriLix.Persistence.IntegrationTest.Initializers;
using Xunit.Abstractions;

namespace UriLix.Persistence.IntegrationTest.Helpers;

internal static class TestHelper
{
    private static DbContextOptions<AppDbContext> GetMsSQLDbOptions(
        string connectionString,
        ITestOutputHelper testOutput)
        => new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .UseAsyncSeeding(async (context, _, _) => await DataInitializer.SeedData(context, default))
            .LogTo(testOutput.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .Options;
    internal static AppDbContext GetAppDbContext(string connectionString, ITestOutputHelper testOutput) 
        => new(GetMsSQLDbOptions(connectionString, testOutput));
}
