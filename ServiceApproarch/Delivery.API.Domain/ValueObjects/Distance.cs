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
}