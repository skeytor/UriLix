namespace UriLix.API.Extensions;

internal static class CorsExtensions
{
    internal const string CORS_POLICY_NAME = "CorsPolicy";
    public static void AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        string[] origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        // Rename the origin name with a more descriptive name
        services.AddCors(options =>
        {
            options.AddPolicy(CORS_POLICY_NAME,
                builder => builder
                    .WithOrigins(origins)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}