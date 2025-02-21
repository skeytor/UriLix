using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UriLix.Domain.Entities;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<ShortenedLink> ShortenedLinks { get; set; }

    public DbSet<ClickStatistic> ClickStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
