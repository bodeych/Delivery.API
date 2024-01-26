using System.ComponentModel.DataAnnotations;

namespace Delivery.API.Domain.Entities;

public sealed class User
{
    [Key] 
    public Guid UserId { get; private set; }
    public string Username { get; private set; } 
    public string Password { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
  
    
    private User(Guid userId, string username, string password, string accessToken, string refreshToken)
    {
        UserId = userId;
        Username = username;
        Password = password;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public static User Create(Guid userId, string username, string password, string accessToken, string refreshToken)
    {
        var user = new User(userId, username, password, accessToken, refreshToken);
        
        return user;
    }
    public void UpdateAccessToken(string newToken)
    {
        AccessToken = newToken;
    }
}
