using Delivery.API.Application.Interfaces;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.Services;

public sealed class DistanceCalculator
{
    public int CalculateDistanceMeters(Coordinate fromPoint, Coordinate toPoint)
    {
        var earthRadiusM = 6371000.0;

        var distanceLatitude = degreesToRadians(toPoint.Latitude - fromPoint.Latitude);
        var distanceLongitude = degreesToRadians(toPoint.Longitude - fromPoint.Longitude);

        var degreesFromPoint = degreesToRadians(fromPoint.Latitude);
        var degreesToPoint = degreesToRadians(toPoint.Latitude);

        var a = Math.Sin(distanceLatitude / 2) * Math.Sin(distanceLatitude / 2) +
                Math.Sin(distanceLongitude / 2) * Math.Sin(distanceLongitude / 2) * Math.Cos(degreesFromPoint) *
                Math.Cos(degreesToPoint);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var result = Convert.ToInt32(earthRadiusM * c);
        
        return result;
    }

    private double degreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}