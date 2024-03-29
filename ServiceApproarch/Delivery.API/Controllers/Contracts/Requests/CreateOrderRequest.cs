using System.Runtime.Serialization;
using Delivery.API.Controllers.Contracts.Shared;

namespace Delivery.API.Controllers.Contracts.Requests;

[DataContract]
public sealed class CreateOrderRequest
{
    [DataMember(Name = "pickup")]
    public Coordinate Pickup { get; init; }
    [DataMember(Name = "dropoff")]
    public Coordinate Dropoff { get; init; }
}
