using Delivery.API.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Delivery.API.Application.Services;

public sealed class CustomerService
{
    private readonly IDataContext _dataContext;
    private readonly IMemoryCache _memoryCache;
    
    public CustomerService(IDataContext dataContext, IMemoryCache memoryCache)
    {
        _dataContext = dataContext;
        _memoryCache = memoryCache;
    }
    
    public async Task<List<OrderService.OrderDetailsDto>> FindById(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"Orders_{id}";

        if (_memoryCache.TryGetValue(cacheKey, out List<OrderService.OrderDetailsDto> cachedOrders))
        {
            return cachedOrders;
        }
        var orders = await _dataContext.Orders
            .AsNoTracking()
            .Where(i => i.UserId == id).ToListAsync();
       

        if (orders is null)
        {
            return null;
        }


        var detailsDto = orders.Select(x => new OrderService.OrderDetailsDto
        {
            Id = x.Id,
            CreatorId = x.UserId,
            Pickup = new OrderService.CoordinateDto
            {
                Latitude = x.Pickup.Latitude,
                Longitude = x.Pickup.Longitude
            },
            Dropoff = new OrderService.CoordinateDto
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