namespace Delivery.API.Application.Dto;

public sealed class CreateOrderDto
{
    public required Guid UserId { get; init; }
    public required CoordinateDto Pickup { get; init; }
    public required CoordinateDto Dropoff { get; init; }
}