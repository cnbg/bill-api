using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using billing.Entities;
using Microsoft.IdentityModel.Tokens;

namespace billing.Services;

public class JwtService(IConfiguration conf) : IJwtService
{
    public string GenerateAccessToken(User user, IEnumerable<string>? perms = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("usr", user.Id.ToString()),
            new("org", user.OrgId.ToString() ?? ""),
            new("typ", user.Type),
            // new("prm", "region-list"),
            // new("prm", "region-view"),
        };

        claims.AddRange(perms?.Select(p => new Claim("prm", p)) ?? []);

        var token = new JwtSecurityToken(
            conf["Jwt:Issuer"],
            conf["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(conf["Jwt:AccessTokenExpMins"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(int length = 32)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
