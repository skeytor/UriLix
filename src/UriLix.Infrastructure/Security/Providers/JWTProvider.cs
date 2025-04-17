using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UriLix.Application.DOTs;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;
using UriLix.Infrastructure.Security.Auth;

namespace UriLix.Infrastructure.Security.Providers;

public sealed class JWTProvider(IOptions<JwtOptions> options) : IJWTProvider
{
    private readonly JwtOptions options = options.Value;
    public JwtAccessTokenResponse GenerateToken(ApplicationUser user)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        ];

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        DateTime expiresAt = DateTime.UtcNow.AddHours(1);
        JwtSecurityToken secureToken = new(
            options.Issuer, 
            options.Audience, 
            claims, 
            null,
            expiresAt,
            signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(secureToken);
        return new JwtAccessTokenResponse(token, expiresAt.Second, Guid.NewGuid().ToString());
    }
}
