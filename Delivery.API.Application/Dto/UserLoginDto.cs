namespace Delivery.API.Application.Dto;

public sealed class UserLoginDto
{
        public required string Username { get; init; }
        public required string Password { get; init; }
}