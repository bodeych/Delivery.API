
using Delivery.API.Domain.Entities;

namespace Delivery.API.Application.Services;

public interface IOrderService
{
    List<Order> GetOrders();

    Order GetOrderById(Guid orderId);

    void CreateOrder(Point pickUp, Point dropOff);
}