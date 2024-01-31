namespace Delivery.API.Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public async Task Create_CreateUser()
    {
        // Arrange
        var userId = new Guid("8d220771-aa54-4164-a361-2cc3c292fee5");
        var username = "username";
        var password = "password";
        var accessToken = "accessToken";
        var refreshToken = "refreshToken";
        
        // Act
        var user = User.Create(userId, username, password, accessToken, refreshToken);

        // Assert
        Assert.Equal(userId, user.UserId);
        Assert.Equal(username, user.Username);
        Assert.Equal(password, user.Password);
        Assert.Equal(accessToken, user.AccessToken);
        Assert.Equal(refreshToken, user.RefreshToken);
    }

    [Fact]
    public async Task UpdateAccessToken_UpdateToken()
    {
        // Arrange
        var userId = new Guid("8d220771-aa54-4164-a361-2cc3c292fee5");
        var test = "test";
        var newToken = "newToken";
        var user = User.Create(userId, test, test, test, test);
        
        // Act
        user.UpdateAccessToken(newToken);
        
        // Assert
        Assert.Equal(newToken, user.AccessToken);
    }
}