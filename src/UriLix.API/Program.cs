using Scalar.AspNetCore;
using UriLix.API.Extensions;
using UriLix.Persistence;
using UriLix.Application;
using UriLix.API.Security.Authentication;
using UriLix.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddIdentityAuthentication(builder.Configuration);

builder.Services
    .AddDatabaseProvider(builder.Configuration)
    .AddRepositories();

builder.Services.AddApplicationServices();

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