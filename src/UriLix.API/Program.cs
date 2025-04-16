using Scalar.AspNetCore;
using UriLix.API.Extensions;
using UriLix.Persistence;
using UriLix.Application;
using UriLix.Domain.Entities;
using UriLix.Infrastructure.Security.Auth;
using UriLix.Infrastructure.Security.Authorization;
using UriLix.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Authorization policies
builder.Services.AddAuthorizationPolicy();

// Add authentication
builder.Services.AddIdentityAuthProvider(builder.Configuration);

builder.Services
    .AddDatabaseProvider(builder.Configuration)
    .AddRepositories();

builder.Services
    .AddApplicationServices()
    .AddCache(builder.Configuration);

builder.Services.AddServicesProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options.Servers = []);
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();

public partial class Program { };