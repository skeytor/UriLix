using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UriLix.Application.Providers;
using UriLix.Domain.Entities;

namespace UriLix.Infrastructure.Security.Auth.Providers;

public class JWTProvider(IOptions<JwtOptions> options) : IJWTProvider
{
    private readonly JwtOptions options = options.Value;
    public string GenerateToken(ApplicationUser user)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        ];

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        JwtSecurityToken secureToken = new(
            options.Issuer, 
            options.Audience, 
            claims, 
            null, 
            DateTime.UtcNow.AddHours(1), 
            signingCredentials);

        string token = new JwtSecurityTokenHandler().WriteToken(secureToken);
        return token;
    }
}
