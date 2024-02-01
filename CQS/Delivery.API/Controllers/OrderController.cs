using Delivery.API.Application.Commands;
using Delivery.API.Application.Dto;
using Delivery.API.Application.Queries;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Controllers.Contracts.Shared;
using Delivery.API.Controllers.Validators;
using Delivery.API.ServiceCollectionExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delivery.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatorOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        new CreateOrderRequestValidator().ValidateAndThrow(request);

        try
        {
            var orderCommand = new CreateOrderCommand
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

            var createdOrderId = await _mediator.Send(orderCommand, cancellationToken);

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
        new GuidRequestValidator().ValidateAndThrow(orderId);

        var idOrderDetailsQuery = new GetOrderDetailsQuery
        {
            Id = orderId
        };

        var order = await _mediator.Send(idOrderDetailsQuery, cancellationToken);

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
        new GuidRequestValidator().ValidateAndThrow(orderId);

        var deleteOrderCommand = new DeleteOrderCommand
        {
            Id = orderId,
            UserId = HttpContext.GetIdUser()
        };
        
        var order = await _mediator.Send(deleteOrderCommand, cancellationToken);

        if (order is false)
        {
            return NotFound();
        }

        return Ok();
    }
}