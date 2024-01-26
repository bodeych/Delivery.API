using Delivery.API.Application.Dto;
using Delivery.API.Application.Services;
using Delivery.API.Controllers.Contracts.Requests;
using Delivery.API.Controllers.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;
    
     [ApiController]
     [Route("api/v1/")]
     public class IdentityControllers : ControllerBase
     {
         private readonly IdentityService _identityService;
         
         public IdentityControllers(IdentityService identityService)
         {
             _identityService = identityService;
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
    
             var loginDto = new UserLoginDto
                 {
                     Username = request.Username,
                     Password = request.Password
                 };
    
                 var loginTokens = await _identityService.Login(loginDto, cancellationToken);
    
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
         public async Task<IActionResult> Registration([FromBody]UserRegistrationRequest request, CancellationToken cancellationToken)
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
             
                 var registarionDto = new UserRegistrationDto
                 {
                     Username = request.Username,
                     Password = request.Password
                 };
    
                 var registratedUser = await _identityService.Register(registarionDto, cancellationToken);
                 
                 if (registratedUser is false)
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
             
             var tokensDto = new TokensDto
             {
                 AccessToken = request.AccessToken,
                 RefreshToken = request.RefreshToken
             };
    
             var newToken = await _identityService.Refresh(tokensDto, cancellationToken);
                 
             if (newToken is null)
             {
                 return BadRequest("Access/Refresh Token is invalid");
             }
    
             return Ok(newToken);
         }
         
}