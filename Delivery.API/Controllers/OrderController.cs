using Delivery.API.Contracts.Requests.Order;
using Delivery.API.Contracts.Responses.Order;
using Microsoft.AspNetCore.Mvc;
using Delivery.API.Application.Services;


namespace Delivery.API.Controllers;

[Route("api/v1/")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_orderService.GetOrders());
    }
    
    //get api v1{orderId}
    [HttpGet("{orderId}")]
    public IActionResult Get([FromRoute]Guid orderId)
    {
        var order = _orderService.GetOrderById(orderId);

        if (order == null)
            return NotFound();
        
        return Ok(order);
    }
    
    //post api v1
    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest orderRequest)
    {
        var order = orderRequest;
        _orderService.CreateOrder();
        _orderService.GetOrders();
        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var locationUrl = $"{baseUrl}/api/{order.Id}";

        var response = new OrderResponse {Id = order.Id};
        return Created(locationUrl, response);
    }
    
    //delete api v1 {orderId}
    [HttpDelete]
    public IActionResult DeleteId()
    {
        return Ok();
    }
}