namespace Delivery.API.Application.Dto;

public sealed class CoordinateDto
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}