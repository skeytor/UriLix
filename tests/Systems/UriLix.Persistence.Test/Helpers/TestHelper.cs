using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace UriLix.Persistence.Test.Helpers;

internal static class TestHelper
{
    private static DbContextOptions<AppDbContext> GetMsSQLDbOptions(
        string connectionString, 
        ITestOutputHelper testOutput)
        => new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .LogTo(testOutput.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .Options;
    internal static AppDbContext GetAppDbContext(string connectionString, ITestOutputHelper testOutput) 
        => new(GetMsSQLDbOptions(connectionString, testOutput));
}
