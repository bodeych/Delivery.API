using Delivery.API.Application.Services;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Controllers.Contracts.Shared;
using Delivery.API.Domain;
using Delivery.API.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delivery.API.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatorOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Request body is invalid or empty.");
        }

        if (request.PickUp is null)
        {
            return BadRequest("Pickup coordinate is invalid or empty.");
        }

        if (request.DropOff is null)
        {
            return BadRequest("Dropoff coordinate is invalid or empty.");
        }

        try
        {
            var orderDto = new OrderService.CreateOrderDto
            {
                // TODO Get proper user id from JWT token
                CreatorId = HttpContext.GetIdUser(),
                Pickup = new OrderService.CoordinateDto
                {
                    Latitude = request.PickUp.Latitude,
                    Longitude = request.PickUp.Longitude
                },
                Dropoff = new OrderService.CoordinateDto
                {
                    Latitude = request.DropOff.Latitude,
                    Longitude = request.DropOff.Longitude
                },
                
            };

            var createdOrderId = await _orderService.Create(orderDto, cancellationToken);

            return Ok(createdOrderId);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (DomainException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> FindById([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
        {
            return BadRequest("Requested ID is invalid or empty.");
        }

        var order = await _orderService.FindById(orderId, cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        var orderDetailsResponse = new OrderDetailsResponse
        {
            Id = order.Id,
            CreatorId = order.CreatorId,
            Pickup = new Coordinate
            {
                Latitude = order.Pickup.Latitude,
                Longitude = order.Pickup.Longitude
            },
            Dropoff = new Coordinate
            {
                Latitude = order.Dropoff.Latitude,
                Longitude = order.Dropoff.Longitude
            },
            DistanceMeters = order.DistanceMeters,
            Cost = order.Cost
        };

        return Ok(orderDetailsResponse);
    }

    [HttpDelete("{orderId:guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
        {
            return BadRequest("Requested ID is invalid or empty.");
        }

        var order = await _orderService.DeleteById(orderId, cancellationToken);

        if (order is false)
        {
            return NotFound();
        }

        return Ok();
    }
}