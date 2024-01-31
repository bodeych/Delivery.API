using System.IdentityModel.Tokens.Jwt;
using Delivery.API.Application.Services;
using Delivery.API.Application.Settings;

namespace Delivery.API.Application.UnitTests.Services;

public class GenerateTokenTests
{
    [Fact]
    public async Task Generate_WithSettingsSecretKey_GenerateAccessTokenWithCorrectClaims()
    {
        // Arrange
        var settings = new JwtSettings
        {
            SecretKey = "qarxcmlxcahildalknadfasdfadsfasdf"
        };
        var generateToken = new GenerateToken(settings);
        var userId = Guid.NewGuid();

        // Act
        var token = generateToken.AccessToken(userId);
        // Decode the token and verify its claims
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadToken(token) as JwtSecurityToken;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(token);
            Assert.NotNull(jwt);
            Assert.True(jwt.Claims.Any(claim => claim.Type == "creatorId" && claim.Value == userId.ToString()));
        });
    }
    
    [Fact]
    public async Task Generate_WithSettingsSecretKey_GenerateAccessToken()
    {
        // Arrange
        var settings = new JwtSettings
        {
            SecretKey = "qarxcmlxcahildalknadfasdfadsfasdf"
        };
        var generateToken = new GenerateToken(settings);

        // Act
        var refreshToken = generateToken.RefreshToken();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken);
        });
    }
}