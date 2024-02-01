namespace Delivery.API.Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public async Task Create_CreateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var username = "username";
        var password = "password";
        var accessToken = "accessToken";
        var refreshToken = "refreshToken";
        
        // Act
        var user = User.Create(userId, username, password, accessToken, refreshToken);

        // Assert
        using var scope = new AssertionScope();
        user.UserId.Should().Be(userId);
        user.Username.Should().Be(username);
        user.Password.Should().Be(password);
        user.AccessToken.Should().Be(accessToken);
        user.RefreshToken.Should().Be(refreshToken);
    }

    [Fact]
    public async Task UpdateAccessToken_UpdateToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var test = "test";
        var newToken = "newToken";
        var user = User.Create(userId, test, test, test, test);
        
        // Act
        user.UpdateAccessToken(newToken);
        
        // Assert
        user.AccessToken.Should().Be(newToken);
    }
}