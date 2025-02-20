using Microsoft.EntityFrameworkCore;
using UriLix.Persistence;

namespace UriLix.API.Extensions;

internal static class MigrationExtension
{
    internal static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
