using Delivery.API.Application.Dto;
using Delivery.API.Domain.Entities;
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace Delivery.API.Application.Services;

using BCrypt.Net;

public class IdentityService : IIdentityService
{
    private readonly IDataContext _dataContext;
    private readonly GenerateToken _generateToken;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public IdentityService(IDataContext dbContext, GenerateToken generateToken, JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters)
    {
        _dataContext = dbContext;
        _generateToken = generateToken;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<TokensDto?> Login(UserLoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == dto.Username, cancellationToken: cancellationToken);

        if (user == null || BCrypt.Verify(dto.Password, user.Password) == false)
        {
            return null;
        }

        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }

    public async Task<bool> Register(UserRegistrationDto dto, CancellationToken cancellationToken)
    {
        var user = dto.Username;

        var existingUser = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == user, cancellationToken: cancellationToken);
        
        if (existingUser != null)
        {
            return false;
        };
        
        var id = Guid.NewGuid();
        var password = BCrypt.HashPassword(dto.Password);
        var accessToken = _generateToken.AccessToken(id);
        var refreshToken = _generateToken.RefreshToken();
        var createdUser = User.Create(id, user, password, accessToken, refreshToken);
        
        _dataContext.Users.Add(createdUser);
        await _dataContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<TokensDto?> Refresh(TokensDto dto, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AccessToken == dto.AccessToken, cancellationToken: cancellationToken);
        var id = user.UserId;
    
        if (user is null || user.RefreshToken != dto.RefreshToken)
        {
            return null;
        }
        
        var newAccessToken = _generateToken.AccessToken(id);
        user.UpdateAccessToken(newAccessToken);
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync(cancellationToken);
        
        var tokensDto = new TokensDto
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        };

        return tokensDto;
    }
}