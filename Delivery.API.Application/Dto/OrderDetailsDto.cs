using Delivery.API.Application.Services;

namespace Delivery.API.Application.Dto;

public sealed class OrderDetailsDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required CoordinateDto Pickup { get; init; }
    public required CoordinateDto Dropoff { get; init; }
    public required int DistanceMeters { get; init; }
    public required decimal Cost { get; init; }
}