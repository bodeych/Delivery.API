using Delivery.API.Application.Commands;
using Delivery.API.Application.Dto;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Delivery.API.Controllers;
    
     [ApiController]
     [Route("api/v1/identity/")]
     public class IdentityControllers : ControllerBase
     {
         private readonly IMediator _mediator;
         
         public IdentityControllers(IMediator mediator)
         {
             _mediator = mediator;
         }
         
         [HttpPost("login")]
         public async Task<IActionResult> LoginUser([FromBody]UserLoginRequest request, CancellationToken cancellationToken)
         {
             if (request is null)
             {
                 return BadRequest("Request body is invalid or empty.");
             }
    
             if (request.Username is null)
             {
                 return BadRequest("Username is invalid or empty.");
             }
    
             if (request.Password is null)
             {
                 return BadRequest("Password is invalid or empty.");
             }
    
             var loginCommand = new LoginUserCommand()
                 {
                     Username = request.Username,
                     Password = request.Password
                 };
    
                 var loginTokens = await _mediator.Send(loginCommand, cancellationToken);
    
                 if (loginTokens is null)
                 {
                     return BadRequest("Username or Password is invalid");
                 }
    
                 var userLoginResponse = new UserLoginResponse
                 {
                     AccessToken = loginTokens.AccessToken,
                     RefreshToken = loginTokens.RefreshToken
                 };
                 
                 return Ok(userLoginResponse);
         }
         
         [HttpPost("registration")]
         public async Task<IActionResult> RegistrationUser([FromBody]UserRegistrationRequest request, CancellationToken cancellationToken)
         {
             if (request is null)
             {
                 return BadRequest("Request body is invalid or empty.");
             }
    
             if (request.Username is null)
             {
                 return BadRequest("Username is invalid or empty.");
             }
    
             if (request.Password is null)
             {
                 return BadRequest("Password is invalid or empty.");
             }
             
             var registrationCommand = new RegistrationUserCommand
             {
                 Username = request.Username,
                 Password = request.Password
             };
    
             var registrationUser = await _mediator.Send(registrationCommand, cancellationToken);
                 
             if (registrationUser is false)
             {
                 return BadRequest("Username already exists");
             }
    
             return Ok("Success");
         }
         
         [HttpPost("refresh")]
         public async Task<IActionResult> Refresh([FromBody]TokenRefreshRequest request, CancellationToken cancellationToken)
         {
             if (request is null)
             {
                 return BadRequest("Request body is invalid or empty.");
             }
    
             if (request.AccessToken is null)
             {
                 return BadRequest("Access Token is invalid or empty.");
             }
    
             if (request.RefreshToken is null)
             {
                 return BadRequest("Refresh Token is invalid or empty.");
             }
             
             var tokenCommand = new RefreshAccessTokenCommand
             {
                 AccessToken = request.AccessToken,
                 RefreshToken = request.RefreshToken
             };
    
             var newToken = await _mediator.Send(tokenCommand, cancellationToken);
                 
             if (newToken is null)
             {
                 return BadRequest("Access/Refresh Token is invalid");
             }
    
             var tokenResponse = new TokenRefreshResponse
             {
                 AccessToken = newToken.AccessToken,
                 RefreshToken = newToken.RefreshToken
             };
             return Ok(tokenResponse);
         }
}