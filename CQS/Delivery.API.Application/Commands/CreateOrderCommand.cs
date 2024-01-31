using Delivery.API.Application.Dto;
using Delivery.API.Application.Interfaces;
using Delivery.API.Application.Settings;
using Delivery.API.Domain.Entities;
using Delivery.API.Domain.ValueObjects;

namespace Delivery.API.Application.Commands;

public sealed class CreateOrderCommand : ICommand<Guid>
{
    public required Guid UserId { get; init; }
    public required CoordinateDto Pickup { get; init; }
    public required CoordinateDto Dropoff { get; init; }
}

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IDataContext _dataContext;
    private readonly OrderSettings _orderSettings;

    public CreateOrderCommandHandler(IDataContext dataContext, OrderSettings orderSettings)
    {
        _dataContext = dataContext;
        _orderSettings = orderSettings;
    }
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var pickup = Coordinate.Create(request.Pickup.Latitude, request.Pickup.Longitude);
        var dropoff = Coordinate.Create(request.Dropoff.Latitude, request.Dropoff.Longitude);

        var order = Order.Create(request.UserId, pickup, dropoff, _orderSettings.CostPerKm);

        _dataContext.Orders.Add(order);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}