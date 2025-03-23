using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UriLix.Domain.Entities;
using UriLix.Persistence.Abstractions;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext, IUnitOfWork
{
    public DbSet<ShortenedUrl> ShortenedUrl { get; set; }

    public DbSet<ClickStatistic> ClickStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
