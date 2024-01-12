using Delivery.API.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

public class CustomerController : Controller
{
    

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}