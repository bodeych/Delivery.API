using System.Runtime.Serialization;
using Delivery.API.Controllers.Contracts.Shared;

namespace Delivery.API.Controllers.Contracts.Responses;

[DataContract]
public sealed class OrderDetailsResponse
{
    [DataMember(Name = "id")]
    public required Guid Id { get; init; }
    [DataMember(Name = "creator_id")]
    public required string CreatorId { get; init; }
    [DataMember(Name = "pickup")]
    public required Coordinate Pickup { get; init; }
    [DataMember(Name = "dropoff")]
    public required Coordinate Dropoff { get; init; }
    [DataMember(Name = "distance_meters")]
    public required int DistanceMeters { get; init; }
    [DataMember(Name = "cost")]
    public required decimal Cost { get; init; }
}

