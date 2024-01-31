using Delivery.API.Application.Dto;
using Delivery.API.Application.Repositories;
using Delivery.API.Domain.Entities;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.Services;

public sealed class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly DistanceCalculator _distanceCalculator;
    private readonly CostCalculator _costCalculator;

    public OrderService(IOrderRepository orderRepository, DistanceCalculator distanceCalculator, CostCalculator costCalculator)
    {
        _orderRepository = orderRepository;
        _distanceCalculator = distanceCalculator;
        _costCalculator = costCalculator;
    }

    public async Task<Guid> Create(CreateOrderDto dto, CancellationToken cancellationToken)
    {
        var pickup = Coordinate.Create(dto.Pickup.Latitude, dto.Pickup.Longitude);
        var dropoff = Coordinate.Create(dto.Dropoff.Latitude, dto.Dropoff.Longitude);

        var distanceMeters = _distanceCalculator.CalculateDistanceMeters(pickup, dropoff);
        var distance = Distance.FromMeters(distanceMeters);

        var cost = _costCalculator.Calculate(distance);

        var order = Order.Create(dto.UserId, pickup, dropoff, distance, cost);

        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }

    public async Task<OrderDetailsDto?> FindById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(id, cancellationToken);

        if (order is null)
        {
            return null;
        }

        var detailsDto = new OrderDetailsDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Pickup = new CoordinateDto
            {
                Latitude = order.Pickup.Latitude,
                Longitude = order.Pickup.Longitude
            },
            Dropoff = new CoordinateDto
            {
                Latitude = order.Dropoff.Latitude,
                Longitude = order.Dropoff.Longitude
            },
            DistanceMeters = order.Distance.Meters,
            Cost = order.Cost
        };

        return detailsDto;
    }

    public async Task<bool> DeleteById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(id, cancellationToken);

        if (order is null || order.UserId != userId)
        {
            return false;
        }

        await _orderRepository.RemoveAsync(order, cancellationToken);

        return true;
    }
}