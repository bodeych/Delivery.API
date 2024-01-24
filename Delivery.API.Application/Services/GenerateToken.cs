using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Delivery.API.Application.Services;

public sealed class GenerateToken
{
    private readonly JwtSettings _jwtSettings;

    public GenerateToken(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string AccessToken(Guid id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("creatorId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddSeconds(50),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        var result = tokenHandler.WriteToken(token);

        return result;
    }

    public string RefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}