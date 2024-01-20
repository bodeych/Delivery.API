namespace Delivery.API.Controllers.Contracts.Responses;

public class AuthFailedResponse
{
    public IEnumerable<string> Errors { get; set; }
}