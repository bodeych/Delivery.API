using Delivery.API.Application.Dto;
using Delivery.API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Application.Queries;

public sealed class GetOrderDetailsQuery : IQuery<OrderDetailsDto?>
{
    public required Guid Id { get; init; }
}

internal sealed class GetOrderDetailsQueryHandler : IQueryHandler<GetOrderDetailsQuery, OrderDetailsDto?>
{
    private readonly IDataContext _dataContext;
    
    public GetOrderDetailsQueryHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OrderDetailsDto?> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await _dataContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        
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
}