using System.Runtime.Serialization;

namespace Delivery.API.Controllers.Contracts.Responses;

[DataContract]
public class CreateOrderResponse
{
    [DataMember(Name = "id")]
    public required Guid Id { get; init; }
}