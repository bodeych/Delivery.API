using Delivery.API.Application.Dto;
using Delivery.API.Domain.Entities;
using Delivery.API.Application.Repositories;


namespace Delivery.API.Application.Services;

using BCrypt.Net;

 public class IdentityService
 {
     private readonly IIdentityRepository _identityRepository;
     private readonly GenerateToken _generateToken;

     public IdentityService(IIdentityRepository identityRepository, GenerateToken generateToken)
     {
         _identityRepository = identityRepository;
         _generateToken = generateToken;
     }

     public async Task<TokensDto?> Login(UserLoginDto dto, CancellationToken cancellationToken)
     {
         var user = await _identityRepository.FindByUsernameAsync(dto.Username, cancellationToken);

         if (user == null || BCrypt.Verify(dto.Password, user.Password) == false)
         {
             return null;
         }

         var tokensDto = new TokensDto
         {
             AccessToken = user.AccessToken,
             RefreshToken = user.RefreshToken
         };

         return tokensDto;
     }

     public async Task<bool> Register(UserRegistrationDto dto, CancellationToken cancellationToken)
     {
         var existingUser = await _identityRepository.FindByUsernameAsync(dto.Username, cancellationToken);
         
         if (existingUser != null)
         {
             return false;
         };
         
         var id = Guid.NewGuid();
         var user = dto.Username;
         var password = BCrypt.HashPassword(dto.Password);
         var accessToken = _generateToken.AccessToken(id);
         var refreshToken = _generateToken.RefreshToken();
         var createdUser = User.Create(id, user, password, accessToken, refreshToken);

         await _identityRepository.AddAsync(createdUser, cancellationToken);
         
         return true;
     }

     public async Task<TokensDto?> Refresh(TokensDto dto, CancellationToken cancellationToken)
     {
         var user = await _identityRepository.FindByTokenAsync(dto.AccessToken, cancellationToken);
         if (user is null || user.RefreshToken != dto.RefreshToken)
         {
             return null;
         }
         
         var id = user.UserId;
         
         var newAccessToken = _generateToken.AccessToken(id);
         user.UpdateAccessToken(newAccessToken);
         await _identityRepository.UpdateAsync(user, cancellationToken);
         
         var tokensDto = new TokensDto
         {
             AccessToken = user.AccessToken,
             RefreshToken = user.RefreshToken
         };

         return tokensDto;
     }
}