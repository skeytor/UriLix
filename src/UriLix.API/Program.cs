using Scalar.AspNetCore;
using UriLix.API.Extensions;
using UriLix.Persistence;
using UriLix.Application;
using UriLix.Infrastructure.Security.Auth;
using UriLix.Infrastructure.Security.Authorization;
using UriLix.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCorsConfig(builder.Configuration);

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

app.UseCors(CorsExtensions.CORS_POLICY_NAME);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { };