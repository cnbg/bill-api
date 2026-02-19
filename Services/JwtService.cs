using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using billing.DTOs;
using billing.Entities;
using Microsoft.IdentityModel.Tokens;

namespace billing.Services;

public class JwtService(IConfiguration conf) : IJwtService
{
    public string GenerateAccessToken(AuthUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("usr", user.Id.ToString()),
            new("org", user.Org?.Id.ToString() ?? ""),
            new("typ", user.Type),
        };

        claims.AddRange(user.Perms.Select(perm => new Claim("prm", perm)));

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
