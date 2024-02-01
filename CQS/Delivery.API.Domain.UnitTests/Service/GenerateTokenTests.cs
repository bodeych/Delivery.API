using System.IdentityModel.Tokens.Jwt;
using Delivery.API.Application.Service;

namespace Delivery.API.Domain.UnitTests.Service;

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
        using var scope = new AssertionScope();
        token.Should().NotBeNull();
        jwt.Should().NotBeNull();
        jwt.Claims.Any(claim => claim.Type == "creatorId" && claim.Value == userId.ToString()).Should().BeTrue();
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
        refreshToken.Should().NotBeNullOrEmpty();
    }
}