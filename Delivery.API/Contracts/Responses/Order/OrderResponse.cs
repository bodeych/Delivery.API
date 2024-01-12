
namespace Delivery.API.Contracts.Responses.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    
    public Point PickUp { get; set; }
    
    public Point DropOff { get; set; }
}

public class Point
{
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}