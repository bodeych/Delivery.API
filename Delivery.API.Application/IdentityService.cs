using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Delivery.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


namespace Delivery.API.Application;

public class IdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(UserManager<IdentityUser> userManager)
    {
        _userManager = _userManager;
    }
    public async Task<AuthenticationResult> RegisterAsync(string login, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(login);
        
        if (existingUser != null)
        {
            return new AuthenticationResult
            {
                Errors = new [] {"User with this name already exists"}
            };
        }

        var newUser = new IdentityUser
        {
            UserName = login,
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult
            {
                Errors = createdUser.Errors.Select((x => x.Description))
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, newUser.UserName),
                new Claim("creatorId", newUser.Id)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthenticationResult
        {
            Success = true,
            Token = tokenHandler.WriteToken(token)
        };
    }
}