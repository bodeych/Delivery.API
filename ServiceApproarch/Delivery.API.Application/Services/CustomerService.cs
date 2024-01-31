using Delivery.API.Application.Dto;
using Delivery.API.Application.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Delivery.API.Application.Services;

public sealed class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMemoryCache _memoryCache;
    
    public CustomerService(ICustomerRepository customerRepository, IMemoryCache memoryCache)
    {
        _customerRepository = customerRepository;
        _memoryCache = memoryCache;
    }
    
    public async Task<List<OrderDetailsDto>?> FindById(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"Orders_{id}";

        if (_memoryCache.TryGetValue(cacheKey, out List<OrderDetailsDto> cachedOrders))
        {
            return cachedOrders;
        }

        var orders = await _customerRepository.FindByIdToListAsync(id);
       

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