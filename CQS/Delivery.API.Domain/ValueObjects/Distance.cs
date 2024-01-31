namespace Delivery.API.Domain.ValueObjects;

public class Distance
{
    private const int Ratio = 1000;
    public int Meters { get; }
    public float Kilometers => Meters / (float)Ratio;

    private Distance(int meters)
    {
        Meters = meters;
    }

    public static Distance FromMeters(int meters)
    {
        if (meters < 0)
        {
            throw new ArgumentException(nameof(meters));
        }

        var distance = new Distance(meters);

        return distance;
    }

    public static Distance FromKilometers(float km)
    {
        if (km < 0)
        {
            throw new ArgumentException(nameof(km));
        }

        var meters = (int)(km * Ratio);
        var distance = new Distance(meters);

        return distance;
    }

    public static Distance CreateFromToPoints(Coordinate fromPoint, Coordinate toPoint)
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
        
        return FromMeters(result);
        
        double degreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}