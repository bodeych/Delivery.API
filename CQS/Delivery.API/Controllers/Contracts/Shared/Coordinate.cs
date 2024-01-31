using System.Runtime.Serialization;

namespace Delivery.API.Controllers.Contracts.Shared;

[DataContract]
public sealed class Coordinate
{
    [DataMember(Name = "latitude")]
    public double Latitude { get; init; }
    [DataMember(Name = "longitude")]
    public double Longitude { get; init; }
}