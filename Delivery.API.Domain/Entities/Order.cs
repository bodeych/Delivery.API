namespace Delivery.API.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Point PickUp { get; set; }
    public Point DropOff { get; set; }
    
}


