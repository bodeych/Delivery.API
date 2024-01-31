using Delivery.API.Application.Dto;
using Delivery.API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Delivery.API.Application.Queries;

public class GetListOrdersQuery : IQuery<List<OrderDetailsDto>?>
{
    public required Guid Id { get; init; }
}

internal sealed class GetListOrdersQueryHandler : IQueryHandler<GetListOrdersQuery, List<OrderDetailsDto>?>
{
    private readonly IDataContext _dataContext;
    private readonly IMemoryCache _memoryCache;

    public GetListOrdersQueryHandler(IDataContext dataContext, MemoryCache memoryCache)
    {
        _dataContext = dataContext;
        _memoryCache = memoryCache;
    }

    public async Task<List<OrderDetailsDto>?> Handle(GetListOrdersQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Orders_{request.Id}";

        if (_memoryCache.TryGetValue(cacheKey, out List<OrderDetailsDto> cachedOrders))
        {
            return cachedOrders;
        }

        var orders = await _dataContext.Orders
            .AsNoTracking()
            .Where(i => i.UserId == request.Id).ToListAsync();

        if (orders.IsNullOrEmpty())
        {
            return null;
        }

        var detailsDto = orders.Select(x => new OrderDetailsDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Pickup = new CoordinateDto
            {
                Latitude = x.Pickup.Latitude,
                Longitude = x.Pickup.Longitude
            },
            Dropoff = new CoordinateDto
            {
                Latitude = x.Dropoff.Latitude,
                Longitude = x.Dropoff.Longitude
            },
            DistanceMeters = x.Distance.Meters,
            Cost = x.Cost,
        }).ToList();

        _memoryCache.Set(cacheKey, detailsDto, TimeSpan.FromMinutes(10));
        
        return detailsDto;
    }
}