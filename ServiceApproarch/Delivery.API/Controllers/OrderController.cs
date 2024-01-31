using Delivery.API.Application.Dto;
using Delivery.API.Application.Services;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Controllers.Contracts.Shared;
using Delivery.API.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delivery.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatorOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Request body is invalid or empty.");
        }

        if (request.Pickup is null)
        {
            return BadRequest("Pickup coordinate is invalid or empty.");
        }

        if (request.Dropoff is null)
        {
            return BadRequest("Dropoff coordinate is invalid or empty.");
        }

        try
        {
            var orderDto = new CreateOrderDto
            {
                UserId = HttpContext.GetIdUser(),
                Pickup = new CoordinateDto
                {
                    Latitude = request.Pickup.Latitude,
                    Longitude = request.Pickup.Longitude
                },
                Dropoff = new CoordinateDto
                {
                    Latitude = request.Dropoff.Latitude,
                    Longitude = request.Dropoff.Longitude
                },
                
            };

            var createdOrderId = await _orderService.Create(orderDto, cancellationToken);

            var orderResponse = new CreateOrderResponse
            {
                Id = createdOrderId
            };

            return Ok(orderResponse);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid orderId, CancellationToken cancellationToken)
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
            UserId = order.UserId,
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

        var order = await _orderService.DeleteById(orderId, HttpContext.GetIdUser(), cancellationToken);

        if (order is false)
        {
            return NotFound();
        }

        return Ok();
    }
}