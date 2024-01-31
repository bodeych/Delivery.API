using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Domain.Entities;

public sealed class Order
{
    private const float DefaultCoefficient = 1f;
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Coordinate Pickup { get; private set; }
    public Coordinate Dropoff { get; private set; }
    public Distance Distance { get; private set; }
    public decimal Cost { get; private set; }
    
    // EF ctor
    private Order(Guid id, Guid userId, Distance distance, decimal cost)
    {
        Id = id;
        UserId = userId;
        Distance = distance;
        Cost = cost;
    }

    private Order(Guid id, Guid userId, Coordinate pickup, Coordinate dropoff, Distance distance, decimal cost)
    {
        Id = id;
        UserId = userId;
        Pickup = pickup;
        Dropoff = dropoff;
        Distance = distance;
        Cost = cost;
    }

    public static Order Create(Guid creatorId, Coordinate pickup, Coordinate dropoff, float? distanceCoef)
    {
        var distance = Distance.CreateFromToPoints(pickup, dropoff);
        var cost = CalculateCost(distance, distanceCoef);
        var order = new Order(Guid.NewGuid(), creatorId, pickup, dropoff, distance, cost);

        return order;
    }
    
    
    private static decimal CalculateCost(Distance distance, float? distanceCoef)
    {
        var coefficient = distanceCoef ?? DefaultCoefficient;

        var result = distance.Kilometers * coefficient;

        var cost = Convert.ToDecimal(result);

        return cost;
    }
}


