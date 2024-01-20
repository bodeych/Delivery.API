using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Settings;
using Delivery.API.Domain.Orders;
using Delivery.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Services;

public sealed class OrderService
{
    private readonly IDataContext _dataContext;
    private readonly DistanceCalculator _distanceCalculator;
    private readonly CostCalculator _costCalculator;

    public OrderService(IDataContext dataContext, DistanceCalculator distanceCalculator, CostCalculator costCalculator)
    {
        _dataContext = dataContext;
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

        var order = Order.Create(dto.CreatorId, pickup, dropoff, distance, cost);

        _dataContext.Orders.Add(order);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }

    public async Task<OrderDetailsDto?> FindById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dataContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (order is null)
        {
            return null;
        }

        var detailsDto = new OrderDetailsDto
        {
            Id = order.Id,
            CreatorId = order.CreatorId,
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

    public async Task<bool> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dataContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (order is null)
        {
            return false;
        }
        
        _dataContext.Orders.Remove(order);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    
    public sealed class CreateOrderDto
    {
        public required Guid CreatorId { get; init; }
        public required CoordinateDto Pickup { get; init; }
        public required CoordinateDto Dropoff { get; init; }
    }

    public sealed class OrderDetailsDto
    {
        public required Guid Id { get; init; }
        public required Guid CreatorId { get; init; }
        public required CoordinateDto Pickup { get; init; }
        public required CoordinateDto Dropoff { get; init; }
        public required int DistanceMeters { get; init; }
        public required decimal Cost { get; init; }
    }

    public sealed class CoordinateDto
    {
        public required double Latitude { get; init; }
        public required double Longitude { get; init; }
    }
}