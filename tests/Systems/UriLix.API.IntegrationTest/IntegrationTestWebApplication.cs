﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using UriLix.API.IntegrationTest.Initializer;
using UriLix.Domain.Entities;
using UriLix.Persistence;

namespace UriLix.API.IntegrationTest;

public class IntegrationTestWebApplication<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .UseSeeding((context, _) =>
                {
                    context.Set<ShortenedUrl>().AddRange(SampleData.ShortenedURLs);
                    context.Set<User>().AddRange(SampleData.Users);
                    context.SaveChanges();
                }));
        });
        builder.UseEnvironment("Development");
    }
    public Task InitializeAsync() => _msSqlContainer.StartAsync();

    public new Task DisposeAsync() => _msSqlContainer.StopAsync();
}
