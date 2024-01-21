using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Domain.Orders;

public sealed class Order
{
    public Guid Id { get; private set; }
    public string CreatorId { get; private set; }
    public Coordinate Pickup { get; private set; }
    public Coordinate Dropoff { get; private set; }
    public Distance Distance { get; private set; }
    public decimal Cost { get; private set; }
    
    // EF ctor
    private Order(Guid id, string creatorId, Distance distance, decimal cost)
    {
        Id = id;
        CreatorId = creatorId;
        Distance = distance;
        Cost = cost;
    }

    private Order(Guid id, string creatorId, Coordinate pickup, Coordinate dropoff, Distance distance, decimal cost)
    {
        Id = id;
        CreatorId = creatorId;
        Pickup = pickup;
        Dropoff = dropoff;
        Distance = distance;
        Cost = cost;
    }

    public static Order Create(string creatorId, Coordinate pickup, Coordinate dropoff, Distance distance, decimal cost)
    {
        var order = new Order(Guid.NewGuid(), creatorId, pickup, dropoff, distance, cost);

        return order;
    }
    
    
}


