using System.Runtime.Serialization;

namespace Delivery.API.Controllers.Contracts.Requests;

[DataContract]
public class UserRegistrationRequest
{
    [DataMember(Name = "username")]
    public string Username { get; init; }
    [DataMember(Name = "password")]
    public string Password { get; init; }
}