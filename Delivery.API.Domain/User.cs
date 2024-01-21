using System.ComponentModel.DataAnnotations;

namespace Delivery.API.Domain;

public class User
{
    [Key] public string UserName { get; set; } = "";
    public bool IsActive { get; set; } = false;
    public string Token { get; set; } = "";
    public string Password { get; set; } = "";

    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
        
        
    }
}

public class LoginUser
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RegisterUser
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}