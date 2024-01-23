using Delivery.API.Application.Services;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Controllers.Contracts.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Delivery.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/{userId:guid}/orders")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController (CustomerService customerService)
    {
        _customerService = customerService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Requested ID is invalid or empty.");
        }

        var orders = await _customerService.FindById(userId, cancellationToken);
        
        if (orders is null)
        {
            return NotFound();
        }

        var orderDetailsResponse = orders.Select(x => new OrderDetailsResponse
        {
            Id = x.Id,
            UserId = x.CreatorId,
            Pickup = new Coordinate
            {
                Latitude = x.Pickup.Latitude,
                Longitude = x.Pickup.Longitude
            },
            Dropoff = new Coordinate
            {
                Latitude = x.Dropoff.Latitude,
                Longitude = x.Dropoff.Longitude
            },
            DistanceMeters = x.DistanceMeters,
            Cost = x.Cost
        }).ToList();
        return Ok(orderDetailsResponse);
    }
}