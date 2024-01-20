using Delivery.API.Application;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Delivery.API.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IdentityService _identityService;

    public IdentityController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("registation")]
    public async Task<IActionResult> Register([FromBody] UserRegistationRequest request)
    {
        var authResponse = await _identityService.RegisterAsync(request.Login, request.Password);

        if (!authResponse.Success)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }
        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token
        });
    }
}