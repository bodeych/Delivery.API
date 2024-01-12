using Delivery.API.Domain.Entities;

namespace Delivery.API.Contracts.Responses.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Point PickUp { get; set; }
    public Point DropOff { get; set; }
}