using Delivery.API.Application.Queries;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Controllers.Contracts.Shared;
using Delivery.API.ServiceCollectionExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/customer/orders")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController (IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        
        var idListOrdersQuery = new GetListOrdersQuery
        {
            Id = HttpContext.GetIdUser()
        };
        var orders = await _mediator.Send(idListOrdersQuery, cancellationToken);
        
        if (orders is null)
        {
            return NotFound();
        }

        var orderDetailsResponse = orders.Select(x => new OrderDetailsResponse
        {
            Id = x.Id,
            UserId = x.UserId,
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