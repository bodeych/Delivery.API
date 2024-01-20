namespace Delivery.API.Controllers.Contracts.Requests;

public class UserRegistationRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}