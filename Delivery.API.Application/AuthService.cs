using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Delivery.API.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Delivery.API.Application.Interfaces;

namespace Delivery.API.Application;

using BCrypt.Net;
public class AuthService
{
    private readonly IDataContext _dataContext;
    private readonly IConfiguration _configuration;
    public AuthService(IDataContext dbContext, IConfiguration configuration)
    {
        _dataContext = dbContext;
        _configuration = configuration;
    }

    public async Task<User> Login(string email, string password)
    {
        User? user = await _dataContext.Users.FindAsync(email);

        if (user == null || BCrypt.Verify(password, user.Password) == false)
        {
            return null; //returning null intentionally to show that login was unsuccessful
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("id", user.UserName),
            }),
            IssuedAt = DateTime.UtcNow,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);
        user.IsActive = true;

        return user;
    }

    public async Task<User> Register(User user,  CancellationToken cancellationToken)
    {
        user.Password = BCrypt.HashPassword(user.Password);
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
            
        return user;
    }
}