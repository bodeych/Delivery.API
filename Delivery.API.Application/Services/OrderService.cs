using Delivery.API.Domain.Entities;


namespace Delivery.API.Application.Services;

public class OrderService : IOrderService
{
    private readonly List<Order> _orders;

    public OrderService()
    {
        _orders = new List<Order>();
        for (var i = 0; i < 5; i++)
        {
            _orders.Add(new Order
            {
                Id = Guid.NewGuid(),
                PickUp = Point.AddCoordinate(50.237036, 19.968614),
                DropOff = Point.AddCoordinate(50.237036, 19.968614)
            });
        }
    }
    
    public List<Order> GetOrders()
    {
        return _orders;
    }

    public Order GetOrderById(Guid orderId)
    {
        return _orders.SingleOrDefault(x => x.Id == orderId);
    }
    
}