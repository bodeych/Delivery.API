using Delivery.API.Application.Dto;
using Delivery.API.Application.Services;

namespace Delivery.API.Application.Interfaces;

public interface IIdentityService
{
    Task<TokensDto?> Login(UserLoginDto dto, CancellationToken cancellationToken);
    
}