using Delivery.API.Domain.Entities;


namespace Delivery.API.Application.Services;

public class OrderService : IOrderService
{
    private static List<Order> _orders = new List<Order>();
    

    public List<Order> GetOrders()
    {
        return _orders;
    }

    public Order GetOrderById(Guid orderId)
    {
        return _orders.SingleOrDefault(x => x.Id == orderId);
    }

    public void CreateOrder(Point pickUp, Point dropOff)
    {
        var point = new Order
        {
            Id = Guid.NewGuid(),
            PickUp = pickUp,
            DropOff = dropOff
        };

        _orders.Add(point);
    }
}