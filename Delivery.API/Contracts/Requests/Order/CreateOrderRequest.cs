
namespace Delivery.API.Contracts.Requests.Order;

public class CreateOrderRequest
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
