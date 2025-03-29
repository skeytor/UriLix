using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Text;
using UriLix.Infrastructure.Security.Auth;

namespace UriLix.Infrastructure.Security.Options;

internal sealed class JwtBearerParametersConfigureOptions(IOptions<JwtOptions> jwtOptions) 
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;
    public void Configure(string? name, JwtBearerOptions options) 
        => Configure(options);

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    }
}
